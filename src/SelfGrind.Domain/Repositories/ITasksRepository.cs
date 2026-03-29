using SelfGrind.Domain.Entities;

namespace SelfGrind.Domain.Repositories;

public interface ITasksRepository
{
    Task<Guid> Create(TaskItem taskItem);
    Task<TaskItem[]> GetAllAsync(string userId, CancellationToken cancellationToken = default);
    Task<TaskItem?> GetByIdAsync(string userId, Guid taskItemId, CancellationToken cancellationToken = default);
    Task<TaskOccurrence[]> GetTodayTasksAsync(string userId, DateOnly today, CancellationToken cancellationToken = default);
    Task UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
    Task ArchiveAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
}