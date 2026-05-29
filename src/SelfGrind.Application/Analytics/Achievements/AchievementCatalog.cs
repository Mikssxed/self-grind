using SelfGrind.Domain.Constants;

namespace SelfGrind.Application.Analytics.Achievements;

public static class AchievementCatalog
{
    public static readonly IReadOnlyList<AchievementDefinition> All =
    [
        new(
            "first_steps",
            "First Steps",
            "Complete your first task",
            "🏃",
            "orange",
            ctx => ctx.TasksCompleted >= 1),
        new(
            "week_warrior",
            "Week Warrior",
            "Complete tasks 7 days in a row",
            "⚔️",
            "blue",
            ctx => ctx.CurrentStreak >= 7),
        new(
            "knowledge_seeker",
            "Knowledge Seeker",
            "Reach Knowledge level 5",
            "📚",
            "green",
            ctx => ctx.StatLevels.TryGetValue(BaseAttribute.Knowledge, out var lvl) && lvl >= 5),
        new(
            "zen_master",
            "Zen Master",
            "Reach Discipline level 10",
            "🧘",
            "purple",
            ctx => ctx.StatLevels.TryGetValue(BaseAttribute.Discipline, out var lvl) && lvl >= 10),
        new(
            "iron_will",
            "Iron Will",
            "Reach overall level 50",
            "💪",
            "crimson",
            ctx => ctx.UserLevel >= 50),
        new(
            "perfect_month",
            "Perfect Month",
            "Reach a 30-day streak",
            "👑",
            "orange",
            ctx => ctx.CurrentStreak >= 30),
    ];
}
