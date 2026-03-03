# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

SelfGrind is a task management application with a gamification angle (tasks have XP and attributes). It consists of a .NET 10 backend and a Vue 3 frontend.

## Commands

### Backend (run from `src/SelfGrind.API`)

```bash
dotnet run                  # Start API (auto-migrates DB on startup)
dotnet watch run            # Start with hot reload
dotnet build                # Build the solution
```

**EF Core migrations** (run from repo root):
```bash
dotnet ef migrations add <MigrationName> --project src/SelfGrind.Infrastructure --startup-project src/SelfGrind.API
dotnet ef database update --project src/SelfGrind.Infrastructure --startup-project src/SelfGrind.API
```

### Frontend (run from `src/SelfGrind.App`)

```bash
npm run dev         # Start Vite dev server (http://localhost:5173)
npm run build       # Type-check + build to wwwroot/
npm run build:api   # Regenerate Kiota API client from backend swagger
```

### Dev environment notes

- Backend runs on `http://localhost:5081` (HTTP) / `https://localhost:5081` (HTTPS)
- Vite proxies `/api` to `http://localhost:5081/` — start the backend first
- Database is SQL Server LocalDB — connection string in `appsettings.Development.json` under key `SelfGrindDb`
- Set `VITE_DISABLE_AUTH=true` in `.env` to bypass route auth guards during development (already set by default)

## Architecture

### Backend — Clean Architecture with CQRS

Four layers with strict dependency direction (API → Application → Domain ← Infrastructure):

- **`SelfGrind.Domain`** — entities (`TaskItem`, `TaskSchedule`, `TaskOccurrence`, `User`), repository interfaces, domain exceptions, and constants (`BaseAttribute`, `TaskRepetitionType`, etc.)
- **`SelfGrind.Application`** — MediatR command/query handlers, FluentValidation validators, AutoMapper profiles, `IUserContext` abstraction. Each feature lives in a folder like `Tasks/Commands/CreateTask/` containing the command, handler, and validator.
- **`SelfGrind.Infrastructure`** — EF Core `SelfGrindDbContext` (extends `IdentityDbContext<User>`), repository implementations, `JwtService`, `EmailService` (writes to file in dev), `TaskAuthorizationService`
- **`SelfGrind.API`** — controllers delegate directly to MediatR; `ErrorHandlingMiddleware` maps domain exceptions to HTTP responses; `WebApplicationBuilderExtensions` registers presentation-layer services (Swagger, auth, CORS, Serilog)

**Adding a new feature:** create `Command`/`Query` + `Handler` + `Validator` in `Application`, add repository method to the interface in `Domain` and implement in `Infrastructure`, then expose via a controller in `API`.

### Frontend — Vue 3 + Composition API

- **`src/api/apiClient/`** — generated Kiota TypeScript client (do not edit manually; regenerate with `npm run build:api`)
- **`src/composables/useApiClient.ts`** — singleton Kiota client factory using `BearerAuthenticationProvider`
- **`src/stores/useAuthStore.ts`** — Pinia store; persists JWT to `localStorage`, exposes `isAuthenticated`, `getAuthHeader()`
- **`src/composables/useAuth.ts`** — TanStack Query mutations for login/register/logout
- **`src/router/index.ts`** — Vue Router with `requiresAuth` and `layoutType` meta; routes use `AppViews` map for lazy-loaded views
- **`src/components/layout/`** — `SidebarLayout` / `AppLayout` switch based on `LayoutType` route meta
- **`src/components/base/`** — shared UI primitives (Button, Drawer, Form, Icon, etc.)
- PrimeVue is used `unstyled: true` — all styling is done via Tailwind CSS v4

**Adding a new route/view:** add the view to `AppViews` in `router/index.ts`, then call `createRoute()` with the appropriate `LayoutType`.

### Frontend component conventions

- **Variant-based styling** — never pass raw CSS classes or hex colors as props. Define a `variant` prop with a union type and map variants to classes inside the component using `Record<Variant, string>`.
- **All colors in theme** — every color must be defined as a token in `src/SelfGrind.App/src/style.css` under `@theme`. Use Tailwind classes like `bg-info-500`, `text-error-500` — never hardcode hex values in components.
- **Static Tailwind classes only** — Tailwind CSS v4 uses static scanning, so dynamic class interpolation (`` `bg-${color}-500` ``) won't work. Always use explicit, complete class strings.
- **Granular components** — each distinct visual element should be its own component. Views compose feature-specific components, which in turn compose base components.
- **Component folders** — shared primitives go in `src/components/base/`, feature-specific components go in `src/components/<feature>/` (e.g., `dashboard/`).
- **Computed properties** — use `computed()` for any derived display values (formatted strings, percentages, etc.) rather than inline template expressions.
- **Type exports** — child components should export their variant/interface types so parents can reference them for prop data.
- **Opacity modifiers** — use Tailwind opacity modifiers (`bg-info-500/30`, `border-accent-500/30`) instead of separate RGBA values.
- **`tailwind-merge`** — use `twMerge()` when components accept external classes that might conflict with internal ones (e.g., `BaseBadge`).

### API Client regeneration

After changing any backend endpoint signature:
1. Build the backend: `dotnet build`
2. Run `npm run build:api` from `src/SelfGrind.App` — this exports swagger from the DLL then runs Kiota to regenerate `src/api/apiClient/`
