using System.Collections.ObjectModel;

namespace SelfGrind.Domain.Entities;

public class Habit
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int TargetValue { get; set; }
    public string? Unit { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    
    public string UserId { get; set; }
    public User User { get; set; }
    public ICollection<HabitEntry>? HabitEntries { get; set; } = new Collection<HabitEntry>();
}