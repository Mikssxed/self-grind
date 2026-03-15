# Architecture

SelfGrind uses **Clean Architecture** with **CQRS** and **MediatR** on the backend, and **Vue 3 + Composition API** on the frontend.

---

## Backend Projects

```
SelfGrind.Domain          ← no dependencies (innermost)
SelfGrind.Application     ← depends on Domain
SelfGrind.Infrastructure  ← depends on Domain (implements Domain interfaces)
SelfGrind.API             ← depends on Application + Infrastructure
```

### SelfGrind.Domain
- **Purpose:** Core business logic. Has zero dependencies on other projects.
- **Contains:**
  - `Entities/` — domain models (`TaskItem`, `TaskSchedule`, `TaskOccurrence`, `User`)
  - `Constants/` — enums (`BaseAttribute`, `TaskRepetitionType`, `TaskOccurrenceStatus`, `UserRoles`)
  - `Exceptions/` — domain exceptions (`NotFoundException`, `ValidationException`, `ForbidException`, `AuthenticationException`)
  - `Interfaces/Repositories/` — repository contracts (`ITasksRepository`)

### SelfGrind.Application
- **Purpose:** Use cases (CQRS). Orchestrates domain objects without knowing infrastructure details.
- **Contains:**
  - `{Feature}/Commands/{Name}/` — `{Name}Command.cs`, `{Name}CommandHandler.cs`, `{Name}CommandValidator.cs`
  - `{Feature}/Queries/{Name}/` — `{Name}Query.cs`, `{Name}QueryHandler.cs`
  - `{Feature}/Dtos/` — AutoMapper profiles
  - `User/` — `IUserContext`, `UserContext`, `CurrentUser` record
  - `Extensions/ServiceCollectionExtensions.cs` — registers MediatR, AutoMapper, FluentValidation, IUserContext

### SelfGrind.Infrastructure
- **Purpose:** Persistence, external services. Implements domain interfaces.
- **Contains:**
  - `Persistence/SelfGrindDbContext.cs` — EF Core context extending `IdentityDbContext<User>`
  - `Repositories/` — repository implementations
  - `Services/` — `JwtService`, `EmailService`, `TaskAuthorizationService`
  - `Migrations/` — EF Core migrations
  - `Extensions/ServiceCollectionExtensions.cs` — registers DbContext, Identity, JWT, repos

### SelfGrind.API
- **Purpose:** HTTP layer. Controllers delegate directly to MediatR.
- **Contains:**
  - `Controllers/` — `TasksController`, `IdentityController`
  - `Middlewares/ErrorHandlingMiddleware.cs` — maps domain exceptions to HTTP responses
  - `Models/ApiOperationResult.cs` — standard response wrapper
  - `Extensions/WebApplicationBuilderExtensions.cs` — registers auth, Swagger, CORS, Serilog

---

## Request Flow (typical feature)

```
HTTP Request
    ↓
Controller (API layer)
    ↓  mediator.Send(command)
MediatR Pipeline
    ↓  FluentValidation behavior runs first
CommandHandler (Application layer)
    ↓  calls IUserContext, IMapper, IRepository
Repository Implementation (Infrastructure layer)
    ↓  EF Core
SelfGrindDbContext → SQL Server
    ↑
Returns result up the chain
    ↑
Controller wraps in ApiOperationResult
    ↑
HTTP Response
```

**Error path:** Domain exceptions (`NotFoundException`, `ForbidException`, `ValidationException`) bubble up through MediatR and are caught by `ErrorHandlingMiddleware`, which maps them to the appropriate HTTP status codes.

---

## Frontend

```
SelfGrind.App/src/
├── views/               ← page-level components (one per route)
├── components/
│   ├── base/            ← shared UI primitives
│   ├── layout/          ← SidebarLayout, AppLayout
│   └── <feature>/       ← feature-specific components
├── composables/         ← useApiClient, useAuth, useForm, useNavigation
├── stores/              ← Pinia stores (useAuthStore)
├── router/              ← Vue Router with AppViews map + createRoute()
├── api/apiClient/       ← Kiota-generated TypeScript client (do not edit)
├── schemas/             ← Zod validation schemas
└── style.css            ← Tailwind v4 + theme tokens
```

Data flow: `View → Feature Component → Base Component`, props flow downward, events flow upward.

---

## Dependency Rules (must not be violated)

| Layer | May depend on |
|-------|--------------|
| Domain | Nothing |
| Application | Domain |
| Infrastructure | Domain |
| API | Application, Infrastructure |
| Frontend | HTTP API endpoints only |
