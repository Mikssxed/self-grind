namespace SelfGrind.Application.Tasks.Dtos;

public class ContributionGridDto
{
    public ContributionDayDto[] Days { get; set; } = [];
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public int TotalDaysActive { get; set; }
    public double ActivePercentage { get; set; }
    public double CompletionRate { get; set; }
    public int[] AvailableYears { get; set; } = [];
}
