using SelfGrind.Domain.Constants;

namespace SelfGrind.Application.Analytics.Achievements;

public class AchievementContext
{
    public int TasksCompleted { get; init; }
    public int CurrentStreak { get; init; }
    public int UserLevel { get; init; }
    public IReadOnlyDictionary<BaseAttribute, int> StatLevels { get; init; } = new Dictionary<BaseAttribute, int>();
}
