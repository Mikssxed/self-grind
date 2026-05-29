namespace SelfGrind.Application.Analytics.Achievements;

public record AchievementDefinition(
    string Key,
    string Label,
    string Subtitle,
    string Emoji,
    string Variant,
    Func<AchievementContext, bool> IsUnlocked);
