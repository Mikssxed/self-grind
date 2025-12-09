using SelfGrind.Domain.Constants;

namespace SelfGrind.Domain.Entities;

public class TaskOccurrence
{
    public Guid Id { get; set; }
    public DateOnly ScheduledDate { get; set; }
    public DateOnly? CompletedDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public TaskOccurrenceStatus Status { get; set; } = TaskOccurrenceStatus.Pending;

    public Guid TaskItemId { get; set; }
    public TaskItem TaskItem { get; set; }
}