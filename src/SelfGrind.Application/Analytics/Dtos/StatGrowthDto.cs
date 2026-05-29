namespace SelfGrind.Application.Analytics.Dtos;

public class StatGrowthDto
{
    public DateOnly[] WeekStarts { get; set; } = [];
    public StatGrowthSeriesDto[] Series { get; set; } = [];
}
