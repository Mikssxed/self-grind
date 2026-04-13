using SelfGrind.Domain.Entities;

namespace SelfGrind.Domain.Repositories;

public interface IHabitsRepository
{
    Task<Guid> Create(Habit habit);
}