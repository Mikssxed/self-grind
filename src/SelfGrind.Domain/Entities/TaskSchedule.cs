using SelfGrind.Domain.Constants;

namespace SelfGrind.Domain.Entities;

public class TaskSchedule
{
    public Guid Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public TaskRepetitionType RepetitionType { get; set; }
    public int RepeatInterval { get; set; }
    public List<DayOfWeek>? DaysOfWeek { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public Guid TaskItemId { get; set; }
    public TaskItem TaskItem { get; set; }
}