using System.Collections.ObjectModel;

namespace SelfGrind.Domain.Entities;

public class HabitEntry
{
    public Guid Id { get; set; }
    public Guid HabitId { get; set; }
    public Habit Habit { get; set; } = null!;
    public DateTime EntryDate { get; set; }
    public int Value { get; set; }
}