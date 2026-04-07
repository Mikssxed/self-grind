# SelfGrind Backend Implementation Plan

## Context

The frontend is built with fully mocked data across 6 main views (Dashboard, DailyTasks, ContributionGrid, Character, Analytics, Community). The backend currently only has **Auth (register/login)** and **CreateTask command**. We need to build out all the backend features to replace mocked data with real API calls.

This is a learning plan — you'll implement it yourself. Tips are included per phase.

---

## Current Backend State

**Already done:**
- Domain entities: `TaskItem`, `TaskSchedule`, `TaskOccurrence`, `User`
- Auth: Register, Login, ConfirmEmail, UpdateUserDetails
- CQRS: `CreateTaskCommand` (only command)
- Repository: `ITasksRepository` with `Create()` method only
- EF migrations: Initial + CreateIdentity

**Not done:** Everything else (queries, updates, deletes, stats, gamification)

---

## Implementation Order (7 Phases)

### Phase 1: Task CRUD — The Foundation
> Everything else depends on tasks working end-to-end.

| Feature                | Type | What to build |
|------------------------|------|---------------|
| ~~Get all user tasks~~ | Query | `GetTasksQuery` → returns list of task DTOs with schedule info |
| ~~Get task by ID~~     | Query | `GetTaskByIdQuery` → single task DTO (throw `NotFoundException` if missing) |
| ~~Update task~~        | Command | `UpdateTaskCommand` → modify title, description, XP, attribute, schedule |
| ~~Delete task~~        | Command | `DeleteTaskCommand` → soft-delete (set `IsArchived = true`) |
| ~~Get today's tasks~~  | Query | `GetTodayTasksQuery` → tasks with occurrences scheduled for today |

**Repository methods to add to `ITasksRepository`:**
- `GetAllByUserId(string userId)`
- `GetById(Guid id)`
- `GetTodayTasks(string userId, DateOnly today)`
- `Update(TaskItem task)`
- `Delete(Guid id)` or `Archive(Guid id)`

**Controller:** Expand `TasksController` with GET, PUT, DELETE endpoints.

**Tips:**
- Start with `GetTasksQuery` — it's the simplest query and teaches you the read side of CQRS
- Use `IUserContext` to scope all queries to the current user (never return another user's tasks)
- For `GetTodayTasks`, query `TaskOccurrence` where `ScheduledDate == today` and include the parent `TaskItem`
- Use `ITaskAuthorizationService` to verify the user owns the task before update/delete
- Create DTOs in `Application/Tasks/Dtos/` — never return domain entities from queries
- Add to the existing `TasksProfile` for AutoMapper mappings

---

### Phase 2: Task Completion & Occurrence Management
> Powers the DailyTasks view — marking tasks done, tracking streaks.

| Feature                 | Type | What to build |
|-------------------------|------|---------------|
| ~~Complete occurrence~~ | Command | `CompleteTaskOccurrenceCommand` → set status to Completed, record CompletedDate |
| ~~Undo completion~~     | Command | `UndoTaskOccurrenceCommand` → revert status to Pending |
| ~~Get daily summary~~   | Query | `GetDailySummaryQuery` → completed count, total XP earned today, streak |

**New concepts needed:**
- Streak calculation: count consecutive days where at least 1 occurrence was completed
- XP earned today: sum `Exp` from all tasks whose occurrence was completed today

**Tips:**
- Keep streak logic in the query handler for now — don't over-engineer with a separate service yet
- `CompletedDate = DateTimeOffset.UtcNow` when completing; `null` when undoing
- The `TaskOccurrenceStatus` enum already exists in Domain — use it
- Validate that the occurrence belongs to the user's task (authorization check)

---

### Phase 3: Habits System
> The DailyTasks view shows habits (Water, Sleep, Meals, Steps) — these are different from tasks.

**Decision needed:** Habits could be:
- (A) A new `Habit` entity with its own table — cleaner separation
- (B) A special type of `TaskItem` — reuses existing infrastructure

**Recommended: Option A** — habits have different behavior (daily tracking with numeric values, no XP, no scheduling).

| Feature | Type | What to build |
|---------|------|---------------|
| Create habit | Command | `CreateHabitCommand` → name, emoji, target value, unit |
| Get user habits | Query | `GetHabitsQuery` → all habits with today's logged value |
| Log habit entry | Command | `LogHabitEntryCommand` → record value for today |
| Update habit | Command | `UpdateHabitCommand` → modify name, target, emoji |
| Delete habit | Command | `DeleteHabitCommand` |

**New entities:**
- `Habit` — Id, Name, Emoji, TargetValue, Unit, UserId
- `HabitEntry` — Id, HabitId, Date, Value

**Tips:**
- New migration needed: `dotnet ef migrations add AddHabits ...`
- New repository: `IHabitsRepository` in Domain, `HabitsRepository` in Infrastructure
- New controller: `HabitsController`
- Keep it simple — habits are just name + daily numeric value

---

### Phase 4: User Stats & XP System
> Powers the Dashboard character stats and XP progression.

| Feature | Type | What to build |
|---------|------|---------------|
| Get user stats | Query | `GetUserStatsQuery` → level, total XP, XP per attribute, streak |
| Get dashboard data | Query | `GetDashboardQuery` → aggregated view (stats + recent achievements + daily summary) |

**New concepts:**
- **Level calculation:** Define a formula (e.g., `level = floor(sqrt(totalXP / 100))` or use a lookup table)
- **Attribute XP:** Sum XP from completed tasks grouped by `BaseAttribute`
- **Store vs compute:** You can either store running totals on the User entity or compute from occurrences each time

**Recommended:** Compute from data for now (simpler, no sync issues). Optimize later if slow.

**Tips:**
- Add properties to `User` entity if you go the stored route: `TotalXp`, `Level`
- Or create a `UserStatsDto` that's computed in the query handler by aggregating `TaskOccurrence` data
- The frontend expects 5 attributes: Strength, Knowledge, Discipline, Energy, Focus — check your `BaseAttribute` enum matches (currently has: Strength, Knowledge, Health, Charisma, Focus, Creativity — may need alignment)
- Consider a `StatsService` in Infrastructure if the calculation logic gets complex

---

### Phase 5: Contribution Grid & Activity History
> Powers the ContributionGrid view (GitHub-style activity calendar).

| Feature | Type | What to build |
|---------|------|---------------|
| Get contribution data | Query | `GetContributionGridQuery` → daily activity levels for a given year |
| Get day detail | Query | `GetDayActivityQuery` → tasks completed on a specific date |

**How it works:**
- Query all `TaskOccurrence` records for the user in the given year
- Group by date, count completions
- Map count to activity level (0-4): 0 tasks = level 0, 1 = level 1, 2-3 = level 2, 4-5 = level 3, 6+ = level 4

**Tips:**
- This is pure read — just aggregate existing occurrence data
- Return a flat list of `{ date, level }` objects — let the frontend handle the grid layout
- Use `.GroupBy(o => o.ScheduledDate)` in your repository query
- Consider pagination by year to keep response sizes reasonable

---

### Phase 6: Analytics
> Powers the Analytics view — charts and statistics.

| Feature | Type | What to build |
|---------|------|---------------|
| Weekly activity | Query | `GetWeeklyActivityQuery` → tasks completed per category per day of current week |
| XP per week | Query | `GetXpPerWeekQuery` → weekly XP totals for last N weeks |
| Stat growth | Query | `GetStatGrowthQuery` → attribute XP over time (for line chart) |
| Life balance | Query | `GetLifeBalanceQuery` → current XP distribution across attributes (for radar chart) |
| Achievements summary | Query | `GetAchievementsQuery` → unlocked achievements list |

**Tips:**
- These are all read-only aggregation queries over existing data
- Group and aggregate `TaskOccurrence` + `TaskItem` data
- `GetLifeBalance` is basically the same data as attribute stats from Phase 4, just shaped for a radar chart
- For achievements: start with hardcoded achievement definitions (e.g., "Complete 10 tasks" = check count) — no need for a full achievement entity yet
- You can define achievement rules as a simple list in Application layer and check against stats

---

### Phase 7: Community Features (Future)
> Leaderboard, friends, party quests — implement last, lowest priority.

This requires:
- Friend/follow relationships (new `Friendship` entity)
- Leaderboard queries across users (careful with performance)
- Party/group quests (new `Party`, `PartyQuest` entities)

**Tip:** Skip this phase until everything else works. Community features add significant complexity (privacy, multi-user queries, real-time status). Focus on the single-player experience first.

---

## Migration Checklist

| Phase | Migration Needed? | Description |
|-------|-------------------|-------------|
| 1 | No | Uses existing entities |
| 2 | No | Uses existing `TaskOccurrence` |
| 3 | **Yes** | New `Habit` + `HabitEntry` tables |
| 4 | Maybe | If adding stored stats to `User` |
| 5 | No | Reads existing data |
| 6 | No | Reads existing data |
| 7 | **Yes** | New friendship/party tables |

---

## API Client Regeneration

After completing each phase's API endpoints:
1. `dotnet build` from backend
2. `npm run build:api` from `src/SelfGrind.App`
3. Update frontend views to call real endpoints instead of using mocked data

---

## Attribute Enum Alignment Warning

Frontend expects: **Strength, Knowledge, Discipline, Energy, Focus**
Backend currently has: **Strength, Knowledge, Health, Charisma, Focus, Creativity**

You'll need to decide which set to use and align both sides. Recommendation: go with the frontend's set since the UI is already built around it. Update `BaseAttribute` enum in Domain and add a migration if the DB has existing data.

---

## General Tips

1. **One feature at a time.** Implement the command/query, test it with Swagger, then move to the next.
2. **Test with Swagger first.** Don't touch frontend until the endpoint works in Swagger UI (`http://localhost:5081/swagger`).
3. **Follow the existing pattern.** Look at `CreateTaskCommand` as your template — every new feature follows the same folder structure.
4. **Use the .claude/skills/ files.** They have step-by-step guides for creating commands, queries, endpoints, and entities.
5. **Don't forget authorization.** Every query/command should scope to the current user via `IUserContext`.
6. **Validators are mandatory.** Even for simple commands, add a FluentValidation validator — it's good practice and the pipeline expects it.
7. **DTOs, not entities.** Never return domain entities from queries. Create DTOs and map with AutoMapper.
8. **Start the backend before frontend.** Run `dotnet watch run` for hot reload while developing.
