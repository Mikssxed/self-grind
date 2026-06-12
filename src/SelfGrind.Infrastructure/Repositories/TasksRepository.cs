using Microsoft.EntityFrameworkCore;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Repositories;
using SelfGrind.Infrastructure.Persistence;
using Task = System.Threading.Tasks.Task;

namespace SelfGrind.Infrastructure.Repositories;

public class TasksRepository(SelfGrindDbContext dbContext) : ITasksRepository
{
    public async Task<Guid> Create(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        dbContext.Tasks.Add(taskItem);
        CreateOccurrencesRange(taskItem);
        await dbContext.SaveChangesAsync(cancellationToken);
        return taskItem.Id;
    }

    public async Task<Guid> AddLoggedActivityAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        dbContext.Tasks.Add(taskItem);
        await dbContext.SaveChangesAsync(cancellationToken);
        return taskItem.Id;
    }

    public async Task<(TaskItem[] Items, int TotalCount)> GetAllAsync(string userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Tasks
            .AsNoTracking()
            .Where(t => t.UserId == userId && !t.IsArchived);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Include(t => t.Schedule)
            .OrderByDescending(t => t.CreatedAt)
            .ThenBy(t => t.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<TaskItem?> GetByIdAsync(string userId, Guid taskItemId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Tasks.Include(t => t.Schedule)
            .Where(t => t.UserId == userId && t.Id == taskItemId && !t.IsArchived)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TaskOccurrence[]> GetTodayTasksAsync(string userId, DateOnly today, CancellationToken cancellationToken = default)
    {
        return await dbContext.TaskOccurrences
            .Include(o => o.TaskItem)
                .ThenInclude(t => t.Schedule)
            .Where(o => o.TaskItem.UserId == userId
                     && o.ScheduledDate == today
                     && !o.TaskItem.IsArchived)
            .ToArrayAsync(cancellationToken);
    }

    public async Task UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        var existingOccurrences = await dbContext.TaskOccurrences
            .Where(o => o.TaskItemId == taskItem.Id)
            .ToListAsync(cancellationToken);
        dbContext.TaskOccurrences.RemoveRange(existingOccurrences);

        CreateOccurrencesRange(taskItem);

        taskItem.UpdatedAt = DateTime.Now;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ArchiveAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        taskItem.IsArchived = true;
        taskItem.ArchivedAt = DateTime.Now;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TaskOccurrence?> GetTaskOccurenceById(string userId, Guid taskOccurenceId, CancellationToken cancellationToken = default)
    {
        return await dbContext.TaskOccurrences
            .Include(o => o.TaskItem)
            .Where(o => o.TaskItem.UserId == userId && o.Id == taskOccurenceId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> GetTodayTotalExp(string userId, DateOnly today, CancellationToken cancellationToken = default)
    {
        var totalExp = await dbContext.TaskOccurrences
            .Where(o => o.TaskItem.UserId == userId
                     && o.ScheduledDate == today
                     && o.Status == TaskOccurrenceStatus.Done)
            .SumAsync(o => o.TaskItem.Exp, cancellationToken);

        return totalExp;
    }

    public async Task<int> GetTotalCompletedCountAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.TaskOccurrences
            .Where(o => o.TaskItem.UserId == userId && o.Status == TaskOccurrenceStatus.Done)
            .CountAsync(cancellationToken);
    }

    public async Task<(DateOnly Date, int Completed)[]> GetDailyCompletionSummaryAsync(string userId, DateOnly today, CancellationToken 
            cancellationToken =
            default)
    {
        var summary = await dbContext.TaskOccurrences
            .AsNoTracking()
            .Where(o => o.TaskItem.UserId == userId && o.ScheduledDate < today)
            .GroupBy(o => o.ScheduledDate)
            .Select(g => new
            {
                Date = g.Key,
                Completed = g.Count(o => o.Status == TaskOccurrenceStatus.Done)
            })
            .OrderByDescending(x => x.Date)
            .ToArrayAsync(cancellationToken);

        return summary.Select(x => (x.Date, x.Completed)).ToArray();
    }

    public async Task<(DateOnly Date, int CompletedCount, int TotalCount)[]> GetYearlyCompletionSummaryAsync(
        string userId, int year, CancellationToken cancellationToken = default)
    {
        var startDate = new DateOnly(year, 1, 1);
        var endDate = new DateOnly(year, 12, 31);

        var summary = await dbContext.TaskOccurrences
            .AsNoTracking()
            .Where(o => o.TaskItem.UserId == userId
                     && o.ScheduledDate >= startDate
                     && o.ScheduledDate <= endDate
                     && !o.TaskItem.IsArchived)
            .GroupBy(o => o.ScheduledDate)
            .Select(g => new
            {
                Date = g.Key,
                CompletedCount = g.Count(o => o.Status == TaskOccurrenceStatus.Done),
                TotalCount = g.Count()
            })
            .OrderBy(x => x.Date)
            .ToArrayAsync(cancellationToken);

        return summary.Select(x => (x.Date, x.CompletedCount, x.TotalCount)).ToArray();
    }

    public async Task<TaskOccurrence[]> GetCompletedOccurrencesForDateAsync(
        string userId, DateOnly date, CancellationToken cancellationToken = default)
    {
        return await dbContext.TaskOccurrences
            .AsNoTracking()
            .Include(o => o.TaskItem)
            .Where(o => o.TaskItem.UserId == userId
                     && o.ScheduledDate == date
                     && o.Status == TaskOccurrenceStatus.Done
                     && !o.TaskItem.IsArchived)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<int[]> GetActiveYearsAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.TaskOccurrences
            .AsNoTracking()
            .Where(o => o.TaskItem.UserId == userId && !o.TaskItem.IsArchived)
            .Select(o => o.ScheduledDate.Year)
            .Distinct()
            .OrderDescending()
            .ToArrayAsync(cancellationToken);
    }

    public async Task<(DateOnly Date, BaseAttribute Attribute, int CompletedCount, int TotalExp)[]> GetCompletedAggregatesAsync(
        string userId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        var aggregates = await dbContext.TaskOccurrences
            .AsNoTracking()
            .Where(o => o.TaskItem.UserId == userId
                     && o.Status == TaskOccurrenceStatus.Done
                     && o.ScheduledDate >= startDate
                     && o.ScheduledDate <= endDate
                     && !o.TaskItem.IsArchived)
            .GroupBy(o => new { o.ScheduledDate, o.TaskItem.Attribute })
            .Select(g => new
            {
                Date = g.Key.ScheduledDate,
                Attribute = g.Key.Attribute,
                CompletedCount = g.Count(),
                TotalExp = g.Sum(o => o.TaskItem.Exp)
            })
            .OrderBy(x => x.Date)
            .ToArrayAsync(cancellationToken);

        return aggregates.Select(x => (x.Date, x.Attribute, x.CompletedCount, x.TotalExp)).ToArray();
    }

    public async Task<(BaseAttribute Attribute, int TotalExp)[]> GetCompletedExpByAttributeAsync(
        string userId, DateOnly beforeDate, CancellationToken cancellationToken = default)
    {
        var totals = await dbContext.TaskOccurrences
            .AsNoTracking()
            .Where(o => o.TaskItem.UserId == userId
                     && o.Status == TaskOccurrenceStatus.Done
                     && o.ScheduledDate < beforeDate
                     && !o.TaskItem.IsArchived)
            .GroupBy(o => o.TaskItem.Attribute)
            .Select(g => new { Attribute = g.Key, TotalExp = g.Sum(o => o.TaskItem.Exp) })
            .ToArrayAsync(cancellationToken);

        return totals.Select(x => (x.Attribute, x.TotalExp)).ToArray();
    }

    public async Task<int> GetTotalEarnedExpAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.TaskOccurrences
            .AsNoTracking()
            .Where(o => o.TaskItem.UserId == userId
                     && o.Status == TaskOccurrenceStatus.Done
                     && !o.TaskItem.IsArchived)
            .SumAsync(o => o.TaskItem.Exp, cancellationToken);
    }

    public async Task<DateOnly[]> GetAllCompletionDatesAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.TaskOccurrences
            .AsNoTracking()
            .Where(o => o.TaskItem.UserId == userId
                     && o.Status == TaskOccurrenceStatus.Done
                     && !o.TaskItem.IsArchived)
            .Select(o => o.ScheduledDate)
            .Distinct()
            .OrderBy(d => d)
            .ToArrayAsync(cancellationToken);
    }

    private void CreateOccurrencesRange(TaskItem taskItem)
    {
        var schedule = taskItem.Schedule;
        var occurrences = new List<TaskOccurrence>();
        var startDate = schedule.StartDate;
        var repetitionInterval = schedule.RepeatInterval == 0 ? 1 : schedule.RepeatInterval;

        var intervalDays = repetitionInterval * (schedule.RepetitionType switch
        {
            TaskRepetitionType.Weekly => 7,
            _ => 1
        });

        if (schedule.RepetitionType == TaskRepetitionType.Weekly)
        {
            while (!schedule.DaysOfWeek!.Contains(startDate.DayOfWeek))
            {
                startDate = startDate.AddDays(1);
            }
        }

        while (startDate <= schedule.EndDate)
        {
            if (schedule.StartDate.Month < startDate.Month)
            {
                break;
            }
            occurrences.Add(new TaskOccurrence
            {
                TaskItemId = taskItem.Id,
                TaskItem = taskItem,
                ScheduledDate =  startDate,
            });
            startDate = startDate.AddDays(intervalDays);
        }
        
        dbContext.TaskOccurrences.AddRange(occurrences);
    }
}