using SelfGrind.Domain.Entities;

namespace SelfGrind.Domain.Repositories;

public interface ITasksRepository
{
    Task<Guid> Create(TaskItem taskItem);
}