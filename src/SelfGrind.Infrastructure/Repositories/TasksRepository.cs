using Microsoft.EntityFrameworkCore;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Repositories;
using SelfGrind.Infrastructure.Persistence;
using Task = System.Threading.Tasks.Task;

namespace SelfGrind.Infrastructure.Repositories;

public class TasksRepository(SelfGrindDbContext dbContext) : ITasksRepository
{
    public async Task<Guid> Create(TaskItem taskItem)
    {
        dbContext.Tasks.Add(taskItem);
        CreateOccurrencesRange(taskItem);
        await dbContext.SaveChangesAsync();
        return taskItem.Id;
    }

    public async Task<TaskItem[]> GetAllAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Tasks
            .Include(t => t.Schedule)
            .Where(t => t.UserId == userId && !t.IsArchived)
            .ToArrayAsync(cancellationToken);
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