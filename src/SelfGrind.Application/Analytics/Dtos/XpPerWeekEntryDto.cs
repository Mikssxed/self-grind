namespace SelfGrind.Application.Analytics.Dtos;

public class XpPerWeekEntryDto
{
    public DateOnly WeekStart { get; set; }
    public int WeekNumber { get; set; }
    public int Xp { get; set; }
}
