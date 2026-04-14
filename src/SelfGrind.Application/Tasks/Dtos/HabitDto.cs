namespace SelfGrind.Application.Tasks.Dtos;

public class HabitDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int TargetValue { get; set; }
    public string? Unit { get; set; }
    public int TodayValue { get; set; }
}