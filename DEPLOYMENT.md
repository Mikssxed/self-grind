# SelfGrind — Deployment Runbook

A step-by-step, do-it-yourself guide to put SelfGrind online cheaply (≈ $0 when idle).

**Architecture in one line:** the whole app is a **single Docker container** — the .NET API serves
the built Vue SPA from `wwwroot` and exposes `/api`. There is no separate frontend host and no CORS
to configure.

**Target stack:**
- **Server/runtime:** Azure Container Apps (scale-to-zero → $0 when nobody's using it)
- **Database:** Azure SQL Database, free serverless tier (auto-pauses when idle)
- **Image registry:** GitHub Container Registry (GHCR), free public images
- **Email:** Brevo SMTP (free tier)
- **Analytics:** Google Analytics 4

Work top to bottom. Replace every `<placeholder>` with your own value. Anything marked
**(secret)** must never be committed to git.

---

## Phase 0 — Accounts & tools (one-time)

- [ ] Create a free **Azure account** → https://azure.microsoft.com/free
- [ ] Create a **Brevo account** (email) → https://www.brevo.com
- [ ] Create a **Google Analytics** account → https://analytics.google.com
- [ ] Have a **GitHub account** (you already push here)
- [ ] Install the **Azure CLI** → https://aka.ms/installazurecli
- [ ] Install / start **Docker Desktop** → https://www.docker.com/products/docker-desktop
- [ ] Log in once:
  ```bash
  az login
  az account set --subscription "<your-subscription-name-or-id>"
  az provider register --namespace Microsoft.App
  az provider register --namespace Microsoft.OperationalInsights
  az extension add --name containerapp --upgrade
  ```

---

## Phase 1 — Google Analytics 4 (get the tracking ID first)

The GA ID is baked into the frontend **at build time**, so you need it before building the image.

- [ ] In Google Analytics → **Admin → Create Property** ("SelfGrind", Web).
- [ ] Add a **Web data stream** with your future site URL (you can edit it later).
- [ ] Copy the **Measurement ID** — it looks like `G-XXXXXXXXXX`.
- [ ] Save it; you'll pass it as `VITE_GA_MEASUREMENT_ID` when building the image (Phase 5).

> Nothing to change in code — `src/SelfGrind.App/src/composables/useTracking.ts` already reads this
> variable and no-ops when it's absent (so local dev stays clean).

---

## Phase 2 — Brevo SMTP (transactional email)

- [ ] In Brevo → **Senders, Domains & Dedicated IPs → Senders** → add and **verify** a sender
      address (e.g. `you@yourdomain.com`, or your gmail — Brevo emails you a confirmation link).
- [ ] In Brevo → **SMTP & API → SMTP**, note these four values:
  - Host: `smtp-relay.brevo.com`
  - Port: `587`
  - **Login** (your Brevo SMTP login) **(secret-ish)**
  - **SMTP key / master password** **(secret)** — click "Generate a new SMTP key"

> Code is already wired for this (`ServiceCollectionExtensions.cs` builds an authenticated TLS
> `SmtpClient` in Production). Registration also won't fail if email ever breaks — it logs and
> continues. You'll supply these as env vars in Phase 6.

---

## Phase 3 — Azure SQL Database (free serverless)

Pick a region that offers the free SQL tier (e.g. `westeurope`, `eastus`). Choose a strong password.

- [ ] Resource group:
  ```bash
  az group create -n selfgrind-rg -l westeurope
  ```
- [ ] SQL server (name must be globally unique):
  ```bash
  az sql server create \
    -n selfgrind-sql-<yourname> \
    -g selfgrind-rg -l westeurope \
    --admin-user sgadmin \
    --admin-password '<Strong!DbPassw0rd>'
  ```
- [ ] Allow Azure services (your Container App) to reach it:
  ```bash
  az sql server firewall-rule create \
    -g selfgrind-rg --server selfgrind-sql-<yourname> \
    -n AllowAzure --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0
  ```
- [ ] Create the **free serverless** database (auto-pauses after 1h idle):
  ```bash
  az sql db create \
    -g selfgrind-rg --server selfgrind-sql-<yourname> \
    -n SelfGrindDb \
    --edition GeneralPurpose --compute-model Serverless \
    --family Gen5 --capacity 1 \
    --use-free-limit --free-limit-exhaustion-behavior AutoPause \
    --auto-pause-delay 60
  ```
- [ ] Build and **save** your connection string **(secret)** — you'll need it in Phase 6. The DB
      schema is created automatically on first startup (EF Core migrations run at boot), so you do
      **not** run migrations manually.
  ```
  Server=tcp:selfgrind-sql-<yourname>.database.windows.net,1433;Database=SelfGrindDb;User ID=sgadmin;Password=<Strong!DbPassw0rd>;Encrypt=True;TrustServerCertificate=False;
  ```

> If `--use-free-limit` errors that the free DB already exists or isn't available in your region,
> either reuse the existing free DB or pick another supported region.

---

## Phase 4 — Update the public URL placeholders (frontend SEO)

The repo ships with `https://selfgrind.example.com` as a placeholder in the social/SEO tags. You'll
get your real URL in Phase 6, but it's easiest to do the replacement now if you already know the
shape (`https://selfgrind.<hash>.<region>.azurecontainerapps.io`) — otherwise come back here after
Phase 6 and rebuild.

Replace **every** occurrence of `https://selfgrind.example.com` with your real `https://...` URL in:
- [ ] `src/SelfGrind.App/index.html` (canonical, `og:url`, `og:image`, `twitter:image`, and the
      JSON-LD `url`)
- [ ] `src/SelfGrind.App/public/sitemap.xml` (3 URLs)
- [ ] `src/SelfGrind.App/public/robots.txt` (the `Sitemap:` line)

Quick find/replace from `src/SelfGrind.App` (PowerShell):
```powershell
$old = 'https://selfgrind.example.com'
$new = 'https://<your-real-fqdn>'
Get-ChildItem index.html, public/sitemap.xml, public/robots.txt | ForEach-Object {
  (Get-Content $_ -Raw) -replace [regex]::Escape($old), $new | Set-Content $_ -NoNewline
}
```

> A custom domain is optional. The default `*.azurecontainerapps.io` URL is fine for LinkedIn.

---

## Phase 5 — Build & push the image to GHCR

Run from the **repo root** (where the `Dockerfile` is). Docker must be running.

- [ ] Create a GitHub **Personal Access Token (classic)** with `write:packages` scope →
      https://github.com/settings/tokens
- [ ] Log in to GHCR:
  ```bash
  echo <YOUR_GITHUB_PAT> | docker login ghcr.io -u <github-username> --password-stdin
  ```
- [ ] Build (bakes in the GA ID) and push:
  ```bash
  docker build --build-arg VITE_GA_MEASUREMENT_ID=<G-XXXXXXXXXX> \
    -t ghcr.io/<github-username>/selfgrind:latest .
  docker push ghcr.io/<github-username>/selfgrind:latest
  ```
- [ ] On GitHub → your profile → **Packages → selfgrind → Package settings → Change visibility →
      Public** (so Azure can pull it without credentials).

> The build runs the SPA build (`npm run build`) and the .NET publish inside the image — you don't
> need Node or .NET locally for this. No Kiota regeneration is needed.

---

## Phase 6 — Create the Container App (the server)

- [ ] Generate a JWT signing secret (32+ chars) **(secret)** — keep it safe:
  ```bash
  openssl rand -base64 48
  ```
- [ ] Create the Container Apps environment:
  ```bash
  az containerapp env create -n selfgrind-env -g selfgrind-rg -l westeurope
  ```
- [ ] Create the app with all secrets + config (replace every `<...>`):
  ```bash
  az containerapp create \
    -n selfgrind -g selfgrind-rg \
    --environment selfgrind-env \
    --image ghcr.io/<github-username>/selfgrind:latest \
    --target-port 8080 --ingress external \
    --min-replicas 0 --max-replicas 1 \
    --secrets \
       db-conn='Server=tcp:selfgrind-sql-<yourname>.database.windows.net,1433;Database=SelfGrindDb;User ID=sgadmin;Password=<Strong!DbPassw0rd>;Encrypt=True;TrustServerCertificate=False;' \
       jwt-secret='<your-48-char-random-secret>' \
       smtp-user='<brevo-login>' \
       smtp-pass='<brevo-smtp-key>' \
    --env-vars \
       ASPNETCORE_ENVIRONMENT=Production \
       ASPNETCORE_FORWARDEDHEADERS_ENABLED=true \
       ConnectionStrings__SelfGrindDb=secretref:db-conn \
       JwtSettings__Secret=secretref:jwt-secret \
       SmtpSettings__Host=smtp-relay.brevo.com \
       SmtpSettings__Port=587 \
       SmtpSettings__EnableSsl=true \
       SmtpSettings__Username=secretref:smtp-user \
       SmtpSettings__Password=secretref:smtp-pass \
       SmtpSettings__SenderEmail=<your-verified-sender@domain> \
       SmtpSettings__SenderName=SelfGrind
  ```
- [ ] Get your live URL:
  ```bash
  az containerapp show -n selfgrind -g selfgrind-rg \
    --query properties.configuration.ingress.fqdn -o tsv
  ```
- [ ] Set the frontend URL (used for confirmation-email links) to that value:
  ```bash
  az containerapp update -n selfgrind -g selfgrind-rg \
    --set-env-vars AppSettings__FrontendUrl=https://<your-fqdn>
  ```

**Env var reference (what each one does):**

| Variable | Purpose |
|---|---|
| `ASPNETCORE_ENVIRONMENT=Production` | Turns on prod behavior (static SPA, real SMTP, secret checks) |
| `ASPNETCORE_FORWARDEDHEADERS_ENABLED=true` | Trusts the ACA proxy's `X-Forwarded-Proto` (prevents an HTTPS redirect loop) |
| `ConnectionStrings__SelfGrindDb` | Azure SQL connection string **(secret)** |
| `JwtSettings__Secret` | Signs auth tokens; 32+ chars or the app refuses to start **(secret)** |
| `AppSettings__FrontendUrl` | Base URL placed in confirmation emails |
| `SmtpSettings__*` | Brevo host/port/credentials/sender for outgoing email |

> Port 8080 and `ASPNETCORE_URLS` are already set inside the Dockerfile — don't change them.

---

## Phase 7 — Verify it works

- [ ] Open `https://<your-fqdn>` → the **landing page** loads.
- [ ] First request after idle is slow (~10–60s: container wake + DB resume). This is expected and
      is the price of $0-idle. Subsequent requests are fast.
- [ ] Click **Get started** → register a new account → you land in the app (login works without
      email confirmation).
- [ ] Check your inbox for the **confirmation email** (and Brevo → Statistics to confirm it sent).
- [ ] In **Google Analytics → Reports → Realtime**, confirm your visit shows up; click around and
      confirm each route registers a page view.
- [ ] Paste your URL into the **LinkedIn Post Inspector**
      (https://www.linkedin.com/post-inspector/) → the preview shows the SelfGrind card + image.
      (Re-run it after any change; LinkedIn caches previews.)
- [ ] (SEO) Submit your site + `sitemap.xml` in **Google Search Console**
      (https://search.google.com/search-console).

---

## Phase 8 — Cost guardrails (do this once)

- [ ] In Azure → **Cost Management → Budgets**, create a budget (e.g. €5/month) with an email alert.
- [ ] Confirm the app shows **Min replicas = 0** (Container App → Scale) so it stops when idle.
- [ ] The SQL free tier with `AutoPause` pauses the DB rather than billing once you hit the monthly
      free grant.

---

## Phase 9 — Custom domain (optional)

Swap the ugly `https://selfgrind.<hash>.<region>.azurecontainerapps.io` URL for your own domain
(e.g. `https://app.yourdomain.com`). Azure issues a **free managed TLS certificate**, so you get
HTTPS at no cost. You need a domain you own and access to its DNS settings.

> A **subdomain** like `app.yourdomain.com` is the easy path (it uses a CNAME). An apex/root domain
> like `yourdomain.com` works too but needs an A record — see the note at the end.

- [ ] Grab the two values you'll need for DNS:
  ```bash
  # Your app's current Azure URL (the CNAME target)
  az containerapp show -n selfgrind -g selfgrind-rg \
    --query properties.configuration.ingress.fqdn -o tsv

  # The domain-ownership verification code
  az containerapp show -n selfgrind -g selfgrind-rg \
    --query properties.customDomainVerificationId -o tsv
  ```
- [ ] At your domain registrar / DNS provider, add **two records** for the subdomain `app`:
  - **CNAME** — name `app`, value = the **fqdn** from above
    (`selfgrind.<hash>.<region>.azurecontainerapps.io`)
  - **TXT** — name `asuid.app`, value = the **customDomainVerificationId** from above
- [ ] Wait a few minutes for DNS to propagate (you can check with `nslookup app.yourdomain.com`).
- [ ] Bind the hostname and create the free managed certificate (one command):
  ```bash
  az containerapp hostname bind \
    -n selfgrind -g selfgrind-rg \
    --hostname app.yourdomain.com \
    --environment selfgrind-env \
    --validation-method CNAME
  ```
- [ ] Verify `https://app.yourdomain.com` loads with a valid certificate (give the cert a few
      minutes to issue).
- [ ] **Point the app at the new URL** (so email links and SEO tags use it):
  ```bash
  az containerapp update -n selfgrind -g selfgrind-rg \
    --set-env-vars AppSettings__FrontendUrl=https://app.yourdomain.com
  ```
- [ ] Redo the **Phase 4** placeholder replacement with `https://app.yourdomain.com`, then rebuild
      and push the image (Phase 5) so the OG/sitemap URLs match the new domain.
- [ ] Update your GA4 data stream URL and re-run the **LinkedIn Post Inspector**.

> **Apex/root domain (`yourdomain.com`)** instead of a subdomain: most DNS providers don't allow a
> CNAME at the root, so use an **A record** pointing to the environment's static IP
> (`az containerapp env show -n selfgrind-env -g selfgrind-rg --query properties.staticIp -o tsv`)
> plus a **TXT** record named `asuid` with the same verification code, then run the same
> `hostname bind` command with `--hostname yourdomain.com`.

---

## Updating the app later

Whenever you change code:

```bash
docker build --build-arg VITE_GA_MEASUREMENT_ID=<G-XXXXXXXXXX> \
  -t ghcr.io/<github-username>/selfgrind:latest .
docker push ghcr.io/<github-username>/selfgrind:latest
az containerapp update -n selfgrind -g selfgrind-rg \
  --image ghcr.io/<github-username>/selfgrind:latest
```

### Or automate it — deploy on every `git push`

The repo already has `.github/workflows/deploy.yml`. Once set up, every push to `main` builds the
image, pushes it to GHCR, and rolls out the new version — no manual Docker/Azure commands.

GitHub needs permission to deploy to Azure. You give it a **service principal** (a robot login for
Azure), scoped to only your resource group, and store its credentials as a GitHub secret.

- [ ] Get your subscription id:
  ```bash
  az account show --query id -o tsv
  ```
- [ ] Create the robot login (least-privilege: Contributor on just `selfgrind-rg`). This prints a
      JSON blob — **copy the whole thing**:
  ```bash
  az ad sp create-for-rbac \
    --name selfgrind-github-deploy \
    --role contributor \
    --scopes /subscriptions/<subscription-id>/resourceGroups/selfgrind-rg \
    --sdk-auth
  ```
  The output looks like:
  ```json
  {
    "clientId": "...",
    "clientSecret": "...",
    "subscriptionId": "...",
    "tenantId": "...",
    ...
  }
  ```
- [ ] In GitHub → your repo → **Settings → Secrets and variables → Actions**:
  - **Secrets** tab → New secret:
    - `AZURE_CREDENTIALS` = the entire JSON blob from the previous step **(secret)**
  - **Variables** tab → New variable (one each):
    - `GA_MEASUREMENT_ID` = `G-XXXXXXXXXX`
    - `ACA_RESOURCE_GROUP` = `selfgrind-rg`
    - `ACA_APP_NAME` = `selfgrind`
- [ ] Push a commit to `main` → watch it deploy under the repo's **Actions** tab.

> The workflow pushes to GHCR using GitHub's built-in token (no extra registry secret needed). Treat
> the service-principal JSON like a password — it can deploy to your resource group. To revoke it
> later: `az ad sp delete --id <clientId>`.

---

## Quick reference — what's already done in the code

- Single-container build (`Dockerfile`, `.dockerignore`) — SPA + API in one image on port 8080.
- DB schema auto-migrates on startup (no manual migration step).
- Authenticated SMTP with non-fatal failure handling.
- SEO: meta + Open Graph/Twitter tags, JSON-LD (WebApplication + FAQ), `sitemap.xml`, `robots.txt`,
  per-route page titles, favicon + `og-image.png`.
- GA4 wired via build-time `VITE_GA_MEASUREMENT_ID` with SPA page-view tracking.
- Public landing page at `/` (logged-in users are redirected to their dashboard).

**The only repo edits you make by hand:** the URL placeholder replacement in Phase 4.
