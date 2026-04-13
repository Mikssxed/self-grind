using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Repositories;
using SelfGrind.Infrastructure.Persistence;

namespace SelfGrind.Infrastructure.Repositories;

public class HabitsRepository(SelfGrindDbContext dbContext) : IHabitsRepository
{
    public async Task<Guid> Create(Habit habit)
    {
        dbContext.Habits.Add(habit);
        await dbContext.SaveChangesAsync();
        return habit.Id;
    }
}