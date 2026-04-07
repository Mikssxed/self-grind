namespace SelfGrind.Application.Tasks.Dtos;

public class DailySummaryDto
{
    public int CompletedCount { get; set; }
    public int TotalExpEarnedToday { get; set; }
    public int Streak { get; set; }
}