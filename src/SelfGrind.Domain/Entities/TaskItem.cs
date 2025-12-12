using System.Collections.ObjectModel;
using SelfGrind.Domain.Constants;

namespace SelfGrind.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public bool IsCompleted { get; set; }
    public int Exp { get; set; }
    public BaseAttribute Attribute { get; set; }
    public bool IsArchived { get; set; }
    public DateTime? ArchivedAt { get; set; }

    public TaskSchedule Schedule { get; set; }
    public ICollection<TaskOccurrence>? Occurrences { get; set; } = new Collection<TaskOccurrence>();
    
    public string UserId { get; set; }
    public User User { get; set; } = default!;
}