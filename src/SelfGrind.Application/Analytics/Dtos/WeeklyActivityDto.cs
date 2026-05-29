namespace SelfGrind.Application.Analytics.Dtos;

public class WeeklyActivityDto
{
    public DateOnly WeekStart { get; set; }
    public WeeklyActivityDayDto[] Days { get; set; } = [];
}
