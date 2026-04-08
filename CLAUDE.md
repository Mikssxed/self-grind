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
- **Computed properties** — use `computed()` for any derived display values (formatted strings, percentages, filtering, mapped arrays, etc.) rather than inline template expressions. No logic of any kind in the template — if there's a `v-for` over transformed data, compute the mapped array first.
- **No inline event handlers with logic** — `@event="handler"` only; move any arrow functions or expressions into named functions in `<script setup>`.
- **Type exports** — child components should export their variant/union types so parents can reference them. Do NOT export interfaces that duplicate Kiota-generated types — use the generated type directly instead.
- **Opacity modifiers** — use Tailwind opacity modifiers (`bg-info-500/30`, `border-accent-500/30`) instead of separate RGBA values.
- **`tailwind-merge`** — use `twMerge()` when components accept external classes that might conflict with internal ones (e.g., `BaseBadge`).
- **Use Kiota types directly** — never define a manual interface or type that duplicates a shape already generated in `src/api/apiClient/models`. Pass `TodayTaskItemDto[]`, `TaskItemDto`, etc. directly as props and do any display-mapping (enum → label/emoji/variant) inside the receiving component or a dedicated composable.
- **Nullable narrowing via computed** — Kiota generates all fields as `T | null | undefined`. Narrow before use with a typed computed: `computed(() => props.items.filter((i): i is Dto & { id: string } => !!i.id))`.

### Enum serialization

All backend enums serialize as **strings** (e.g., `"Strength"` not `0`). This is configured via `JsonStringEnumConverter` in `WebApplicationBuilderExtensions.cs`. Kiota generates them as TypeScript const objects:

```typescript
export const BaseAttributeObject = { Strength: "Strength", Knowledge: "Knowledge", ... } as const;
export type BaseAttribute = (typeof BaseAttributeObject)[keyof typeof BaseAttributeObject];
```

Always compare against the generated enum object constants, never raw string literals:

```typescript
import { TaskOccurrenceStatusObject, BaseAttributeObject } from '@/api/apiClient/models';

// CORRECT
item.occurrenceStatus === TaskOccurrenceStatusObject.Done
item.attribute === BaseAttributeObject.Strength

// WRONG — raw string literals
item.occurrenceStatus === 'Done'
item.attribute === 'Strength'
```

### API Client regeneration

After changing any backend endpoint signature or adding/modifying enums:
1. Stop the running backend (the process locks the DLL)
2. Build: `dotnet build` from repo root or the API project
3. Run `npm run build:api` from `src/SelfGrind.App` — exports swagger from the DLL then runs Kiota to regenerate `src/api/apiClient/`

---

## Claude Workflow

**Always follow this two-phase approach for any non-trivial task:**

### Phase 1 — Plan
Before writing code, produce a plan that includes:
- Files to create (with full paths)
- Files to modify (with full paths)
- Layers affected (Domain / Application / Infrastructure / API / Frontend)
- CQRS operations needed (commands, queries)
- API changes (new/modified endpoints)
- Frontend changes (new views, components, route updates)
- EF Core migration needed (yes/no)
- API client regeneration needed (yes/no)

Wait for user approval before implementing.

### Phase 2 — Implement
After approval, implement in this order:
1. Domain entity + repository interface (if new)
2. Application command/query + handler + validator
3. AutoMapper profile (if mapping needed)
4. Infrastructure repository implementation + DI registration
5. API controller action
6. EF Core migration (if schema changed)
7. Frontend view + component(s) + route registration
8. Kiota client regeneration (if endpoint changed)

---

## CQRS Rules

| Rule | Detail |
|------|--------|
| Commands modify state | Create, update, delete operations |
| Queries are read-only | No side effects, no state changes |
| Commands return minimal data | ID (`Guid`) or void (`IRequest`) |
| Queries return DTOs | Never return raw domain entities |
| All requests go through MediatR | Controllers call `mediator.Send()` only |
| Handlers live in Application | Never in Domain, Infrastructure, or API |
| Validation runs before handler | FluentValidation via MediatR pipeline |

---

## Layer Rules (strict — do not violate)

```
Domain      → no dependencies on any other layer
Application → depends on Domain only
Infrastructure → depends on Domain only (implements Domain interfaces)
API         → depends on Application + Infrastructure
Frontend    → calls HTTP API endpoints only
```

**Application handlers must not:**
- Reference `DbContext`, `SignInManager`, or any EF Core type directly
- Import from `SelfGrind.Infrastructure` namespace
- Contain SQL queries or persistence logic

**Domain entities must not:**
- Import from Application, Infrastructure, or API namespaces
- Call any services

---

## Naming Quick Reference

| Thing | Pattern | Example |
|-------|---------|---------|
| Command | `{Verb}{Feature}Command` | `CreateTaskCommand` |
| Handler | `{Verb}{Feature}CommandHandler` | `CreateTaskCommandHandler` |
| Query | `{Verb}{Feature}Query` | `GetTasksQuery` |
| Validator | `{Verb}{Feature}CommandValidator` | `CreateTaskCommandValidator` |
| Repository interface | `I{Feature}Repository` | `ITasksRepository` |
| Repository impl | `{Feature}Repository` | `TasksRepository` |
| AutoMapper profile | `{Feature}Profile` | `TasksProfile` |
| Vue view | `{Feature}View.vue` | `DashboardView.vue` |
| Base component | `Base{Name}.vue` | `BaseButton.vue` |
| Feature component | `{Feature}{Name}.vue` | `DashboardCharacter.vue` |

---

## .claude/ Reference Files

Detailed context, skills, and code templates are in `.claude/`:

```
.claude/
├── context/
│   ├── architecture.md       — layer diagram, request flow, dependency rules
│   ├── backend-rules.md      — CQRS, MediatR, FluentValidation, AutoMapper patterns
│   ├── frontend-rules.md     — Vue conventions, routing, Pinia, Tailwind rules
│   └── project-overview.md   — tech stack, repo layout, key files
├── skills/
│   ├── create-backend-feature.md    — full feature walkthrough (entity→API)
│   ├── create-cqrs-command.md       — Command + Handler + Validator
│   ├── create-cqrs-query.md         — Query + Handler
│   ├── create-api-endpoint.md       — Controller action patterns
│   ├── create-domain-entity.md      — Entity + repository interface
│   ├── create-frontend-view.md      — Vue view + route registration
│   ├── create-frontend-component.md — Variant-based Vue component
│   ├── refactor-backend-feature.md  — Fix layer violations, enforce CQRS
│   └── refactor-frontend-component.md — Fix styling, extract components
└── templates/
    ├── domain-entity.cs      — entity class template
    ├── cqrs-command.cs       — command request template
    ├── cqrs-query.cs         — query request + DTO template
    ├── cqrs-handler.cs       — handler template (command or query)
    ├── api-endpoint.cs       — controller template
    ├── vue-component.vue     — Vue component with variant props
    └── frontend-view.vue     — Vue view template
```
