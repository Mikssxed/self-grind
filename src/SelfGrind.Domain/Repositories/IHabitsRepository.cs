using SelfGrind.Domain.Entities;

namespace SelfGrind.Domain.Repositories;

public interface IHabitsRepository
{
    Task<Guid> Create(Habit habit);
    Task<Habit[]> GetAllAsync(string userId, CancellationToken cancellationToken = default);
    Task<Habit?> GetByIdAsync(string userId, Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Habit habit, CancellationToken cancellationToken = default);
    Task DeleteAsync(Habit habit, CancellationToken cancellationToken = default);
    Task UpsertEntryAsync(HabitEntry entry, CancellationToken cancellationToken = default);
}