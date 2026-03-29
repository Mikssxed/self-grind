using SelfGrind.Domain.Constants;

namespace SelfGrind.Application.Tasks.Dtos;

public class TodayTaskItemDto
{
    public Guid OccurrenceId { get; set; }
    public DateOnly ScheduledDate { get; set; }
    public TaskOccurrenceStatus OccurrenceStatus { get; set; }

    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int Exp { get; set; }
    public BaseAttribute Attribute { get; set; }
    public bool IsCompleted { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public TaskRepetitionType RepetitionType { get; set; }
    public int RepeatInterval { get; set; }
    public List<DayOfWeek>? DaysOfWeek { get; set; }
}
