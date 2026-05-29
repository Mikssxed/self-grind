namespace SelfGrind.Application.Analytics.Dtos;

public class AchievementsDto
{
    public AchievementDto[] Achievements { get; set; } = [];
    public int UnlockedCount { get; set; }
    public int TotalCount { get; set; }
}
