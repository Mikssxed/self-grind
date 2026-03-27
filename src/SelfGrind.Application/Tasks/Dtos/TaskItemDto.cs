using System.Collections.ObjectModel;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Entities;

namespace SelfGrind.Application.Tasks.Dtos;

public class TaskItemDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsCompleted { get; set; }
    public int Exp { get; set; }
    public BaseAttribute Attribute { get; set; }
    public bool IsArchived { get; set; }
    public DateTime? ArchivedAt { get; set; }
    
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public TaskRepetitionType RepetitionType { get; set; }
    public int RepeatInterval { get; set; }
    public List<DayOfWeek>? DaysOfWeek { get; set; } = [];
    
}