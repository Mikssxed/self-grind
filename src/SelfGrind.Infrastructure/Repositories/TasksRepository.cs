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
    
    private void CreateOccurrencesRange(TaskItem taskItem)
    {
        var schedule = taskItem.Schedule;
        var occurrences = new List<TaskOccurrence>();
        var startDate = schedule.StartDate;
        
        var intervalDays = schedule.RepeatInterval * (schedule.RepetitionType switch
        {
            TaskRepetitionType.Once => 0,
            TaskRepetitionType.Daily => 1,
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