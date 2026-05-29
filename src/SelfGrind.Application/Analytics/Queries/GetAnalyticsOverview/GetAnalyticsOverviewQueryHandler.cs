using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Analytics.Achievements;
using SelfGrind.Application.Analytics.Dtos;
using SelfGrind.Application.User;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Analytics.Queries.GetAnalyticsOverview;

public class GetAnalyticsOverviewQueryHandler(
    ILogger<GetAnalyticsOverviewQueryHandler> logger,
    ITasksRepository tasksRepository,
    IUsersRepository usersRepository,
    IUserContext userContext)
    : IRequestHandler<GetAnalyticsOverviewQuery, AnalyticsOverviewDto>
{
    public async Task<AnalyticsOverviewDto> Handle(GetAnalyticsOverviewQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var today = DateOnly.FromDateTime(DateTime.Today);

        logger.LogInformation("Handling GetAnalyticsOverviewQuery for user {UserId}", currentUser.Id);

        var totalXp = await tasksRepository.GetTotalEarnedExpAsync(currentUser.Id, cancellationToken);
        var tasksDone = await tasksRepository.GetTotalCompletedCountAsync(currentUser.Id, cancellationToken);
        var completionDates = await tasksRepository.GetAllCompletionDatesAsync(currentUser.Id, cancellationToken);
        var user = await usersRepository.GetWithStatsAsync(currentUser.Id, cancellationToken);

        var bestStreak = StreakUtil.LongestRun(completionDates);
        var currentStreak = StreakUtil.CurrentRunEndingAt(completionDates, today);

        var statLevels = user?.Stats.ToDictionary(s => s.Attribute, s => s.Level)
                         ?? new Dictionary<BaseAttribute, int>();

        var context = new AchievementContext
        {
            TasksCompleted = tasksDone,
            CurrentStreak = currentStreak,
            UserLevel = user?.Level ?? 1,
            StatLevels = statLevels,
        };

        var unlocked = AchievementCatalog.All.Count(a => a.IsUnlocked(context));

        return new AnalyticsOverviewDto
        {
            TotalXp = totalXp,
            TasksDone = tasksDone,
            AchievementsUnlocked = unlocked,
            AchievementsTotal = AchievementCatalog.All.Count,
            BestStreak = bestStreak,
        };
    }
}
