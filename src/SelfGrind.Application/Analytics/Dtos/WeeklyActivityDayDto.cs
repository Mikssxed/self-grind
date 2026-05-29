namespace SelfGrind.Application.Analytics.Dtos;

public class WeeklyActivityDayDto
{
    public DateOnly Date { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public AttributeCountDto[] Counts { get; set; } = [];
}
