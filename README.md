# SelfGrind

A **gamified task manager** built with .NET 10 and Vue 3. Tasks earn XP and level up your character's attributes, turning everyday to-dos into an RPG-style progression system.

> Live demo: https://selfgrind.ambitiousforest-bc72b9a7.westeurope.azurecontainerapps.io

## Features

- **Tasks with XP & attributes** — every task grants experience and develops one of six attributes (Strength, Knowledge, Health, Discipline, Focus, Energy)
- **Daily quests & habit tracking** — recurring tasks (daily / weekly / one-time) with streaks
- **Character progression** — leveling, evolution tiers, skill trees, inventory/equipment
- **Contribution grid** — a GitHub-style activity heatmap of your completions
- **Analytics** — XP over time, weekly activity, life-balance, stat growth
- **Community** — leaderboard and party quests
- **Auth** — registration, email confirmation, JWT login with refresh tokens

## Tech Stack

- **Backend**: .NET 10, Clean Architecture + CQRS (MediatR), FluentValidation, AutoMapper, EF Core, ASP.NET Identity, JWT, Serilog
- **Frontend**: Vue 3 (Composition API) + TypeScript, Vite, Pinia, TanStack Query, Tailwind CSS v4, PrimeVue (unstyled)
- **Database**: SQL Server (LocalDB in dev)
- **API client**: TypeScript client generated from Swagger with Microsoft Kiota
- **Deployment**: Docker → Azure Container Apps, CI/CD via GitHub Actions (see [DEPLOYMENT.md](DEPLOYMENT.md))

## Prerequisites (What to Install on a Clean Computer)

### Required Software

1. **.NET 10 SDK**
   - Download from: https://dotnet.microsoft.com/download/dotnet/10.0
   - Verify installation: `dotnet --version`

2. **SQL Server LocalDB** (comes with Visual Studio or standalone)
   - **Option A**: Install Visual Studio 2022 (Community Edition is free)
     - During installation, select "Data storage and processing" workload
   - **Option B**: Download standalone SQL Server Express LocalDB
     - Download from: https://aka.ms/sslocaldbinstaller
   - Verify installation: `sqllocaldb info`

3. **Node.js** (version **20 or higher**; 22 LTS recommended — Vite 7 requires Node 20.19+ / 22.12+)
   - Download from: https://nodejs.org/
   - Verify installation: `node --version` and `npm --version`

### Optional Tools

- **Visual Studio 2022** or **JetBrains Rider** (recommended for backend development)
- **VS Code** (recommended for frontend development)
- **SQL Server Management Studio (SSMS)** for database management

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd SelfGrind
```

### 2. Backend Setup

```bash
cd src/SelfGrind.API
dotnet restore
dotnet run
```

The API starts at:

- HTTP: `http://localhost:5081`
- HTTPS: `https://localhost:7130`
- Swagger UI (development only): `http://localhost:5081/swagger`

The database is created and migrated **automatically on first run**. The connection string lives in `appsettings.Development.json` under `ConnectionStrings:SelfGrindDb` (defaults to SQL Server LocalDB):

```json
"ConnectionStrings": {
  "SelfGrindDb": "Server=(localdb)\\mssqllocaldb;Database=SelfGrindDb;Trusted_Connection=True;"
}
```

In development, a seed user is created: **`dev@selfgrind.local` / `Dev123!Pass`**.

### 3. Frontend Setup

```bash
cd src/SelfGrind.App
npm install
npm run dev
```

The app starts at `http://localhost:5173`. Vite proxies `/api` to `http://localhost:5081`, so **start the backend first**.

## Environment Variables (Frontend)

The frontend reads Vite env vars from `src/SelfGrind.App/`:

- **`.env`** — committed shared defaults (`BASE_URL`, `VITE_DISABLE_AUTH`). Do **not** put personal/secret values here.
- **`.env.local`** — your personal overrides; **git-ignored**, loaded after `.env` and takes precedence.

| Variable | Purpose |
|----------|---------|
| `VITE_DISABLE_AUTH` | `true` bypasses route auth guards so you can navigate the app without logging in (dev only). |
| `VITE_GA_MEASUREMENT_ID` | Google Analytics 4 ID. Absent locally → analytics is a no-op. In production it's injected at Docker build time (build arg). Put it in `.env.local` for local testing. |
| `BASE_URL` | App base path (default `/`). |

## Project Structure

```
SelfGrind/
├── src/
│   ├── SelfGrind.API/            # Web API (entry point), controllers, middleware
│   ├── SelfGrind.Application/    # CQRS commands/queries, handlers, validators, mappers
│   ├── SelfGrind.Domain/         # Entities, repository interfaces, constants
│   ├── SelfGrind.Infrastructure/ # EF Core DbContext, repositories, JWT/email services
│   └── SelfGrind.App/            # Vue 3 frontend
├── Dockerfile                    # Multi-stage build (SPA + API)
├── DEPLOYMENT.md                 # Azure / Docker deployment guide
└── SelfGrind.sln                 # Solution file
```

## Architecture

- **Backend**: Clean Architecture with strict layer direction (API → Application → Domain ← Infrastructure) and CQRS via MediatR. Validation runs in a MediatR pipeline behavior before handlers.
- **Frontend**: Vue 3 Composition API, data fetching via TanStack Query composables, state in Pinia, styling with Tailwind v4 theme tokens (PrimeVue used unstyled).
- **Enums** serialize as strings end-to-end; Kiota generates them as const objects on the frontend.

See [`CLAUDE.md`](CLAUDE.md) and the `.claude/` folder for detailed conventions and feature-authoring guides.

## Common Commands

### Backend

```bash
cd src/SelfGrind.API
dotnet run              # Run the API (auto-migrates DB on startup)
dotnet watch run        # Run with hot reload
dotnet build            # Build the solution

# EF Core migrations (run from repo root)
dotnet ef migrations add <Name> --project src/SelfGrind.Infrastructure --startup-project src/SelfGrind.API
dotnet ef database update --project src/SelfGrind.Infrastructure --startup-project src/SelfGrind.API
```

### Frontend

```bash
cd src/SelfGrind.App
npm run dev             # Start Vite dev server (http://localhost:5173)
npm run build           # Type-check (vue-tsc) + build to wwwroot/
npm run preview         # Preview the production build
npm run build:api       # Regenerate the Kiota API client from backend Swagger
```

## Regenerating the API Client

After changing any backend endpoint signature or enum, regenerate the typed frontend client.

**One-time prerequisites** (global dotnet tools):

```bash
# Version MUST match the Swashbuckle.AspNetCore PackageReference in SelfGrind.API.csproj (currently 10.1.2)
dotnet tool install -g Swashbuckle.AspNetCore.Cli --version 10.1.2
dotnet tool install -g Microsoft.OpenApi.Kiota
```

**Steps:**

1. Stop the running backend (the process locks the DLL).
2. `dotnet build` (from repo root or the API project).
3. From `src/SelfGrind.App`, run `npm run build:api`.

> **Gotcha:** `swagger tofile` (the first `build:api` step) fails with a `Startup`-type error unless the environment is Development. Set `ASPNETCORE_ENVIRONMENT=Development` in your shell before running `npm run build:api`.

## Deployment

The app ships as a single container (the .NET API serves the built SPA from `wwwroot`). A multi-stage `Dockerfile` builds both, and a GitHub Actions workflow builds/pushes the image and deploys to Azure Container Apps. `VITE_GA_MEASUREMENT_ID` is passed as a build arg. Full instructions are in [DEPLOYMENT.md](DEPLOYMENT.md).

## Development Workflow

1. **Start the backend first**: `cd src/SelfGrind.API && dotnet run`
2. **Then start the frontend**: `cd src/SelfGrind.App && npm run dev`
3. **Open the app**: `http://localhost:5173`

The database is created and migrated automatically on first run.

## Troubleshooting

### Database

- **LocalDB not found**: install SQL Server Express LocalDB (see Prerequisites).
- **Migration errors**: the app auto-migrates on startup; to run manually:
  ```bash
  dotnet ef database update --project src/SelfGrind.Infrastructure --startup-project src/SelfGrind.API
  ```

### Backend

- **Port already in use**: change ports in `Properties/launchSettings.json`.
- **JWT errors**: ensure `JwtSettings` are configured in `appsettings.Development.json`.

### Frontend

- **API connection errors**: ensure the backend is running on `http://localhost:5081`.
- **Module not found**: run `npm install` again.
- **Port 5173 in use**: Vite automatically tries the next available port.
- **`build:api` fails**: see the Development-environment gotcha in [Regenerating the API Client](#regenerating-the-api-client).

## Configuration Files

- `appsettings.json` / `appsettings.Development.json` — backend settings (connection strings, JWT).
- `src/SelfGrind.App/vite.config.ts` — Vite config with the `/api` proxy to the backend.
- `src/SelfGrind.App/.env` / `.env.local` — frontend environment variables (see above).

## Contributing

1. Create a feature branch.
2. Make your changes (follow the conventions in `CLAUDE.md`).
3. Test both backend (`dotnet build`) and frontend (`npm run build`).
4. Submit a pull request.

## License

Released under the [MIT License](LICENSE).
