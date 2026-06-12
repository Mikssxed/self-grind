using Microsoft.EntityFrameworkCore;
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

    public async Task<(Habit[] Items, int TotalCount)> GetAllWithEntriesForDayAsync(string userId, DateTime day, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var dayStart = day.Date;
        var dayEnd = dayStart.AddDays(1);

        var query = dbContext.Habits
            .AsNoTracking()
            .Where(t => t.UserId == userId);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Include(t => t.HabitEntries!.Where(e => e.EntryDate >= dayStart && e.EntryDate < dayEnd))
            .OrderBy(t => t.CreatedAt)
            .ThenBy(t => t.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Habit?> GetByIdAsync(string userId, Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Habits
            .FirstOrDefaultAsync(h => h.UserId == userId && h.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Habit habit, CancellationToken cancellationToken = default)
    {
        dbContext.Habits.Update(habit);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Habit habit, CancellationToken cancellationToken = default)
    {
        dbContext.Habits.Remove(habit);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpsertEntryAsync(HabitEntry entry, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.HabitEntries
            .FirstOrDefaultAsync(e => e.HabitId == entry.HabitId && e.EntryDate == entry.EntryDate, cancellationToken);

        if (existing is not null)
        {
            if (entry.Value == 0)
                dbContext.HabitEntries.Remove(existing);
            else
                existing.Value = entry.Value;
        }
        else if (entry.Value > 0)
        {
            dbContext.HabitEntries.Add(entry);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}