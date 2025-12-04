using SelfGrind.Domain.Constants;

namespace SelfGrind.Domain.Entities;

public class Task
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public TaskType Type { get; set; }
    public int Exp { get; set; }
    public BaseAttribute Attribute { get; set; }
    
    
    public Guid TaskSourceId { get; set; }
    public required TaskSource TaskSource { get; set; }
}