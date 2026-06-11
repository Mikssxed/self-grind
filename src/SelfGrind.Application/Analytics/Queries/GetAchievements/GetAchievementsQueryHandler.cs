using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Analytics.Achievements;
using SelfGrind.Application.Analytics.Dtos;
using SelfGrind.Application.User;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Analytics.Queries.GetAchievements;

public class GetAchievementsQueryHandler(
    ILogger<GetAchievementsQueryHandler> logger,
    ITasksRepository tasksRepository,
    IUsersRepository usersRepository,
    IUserContext userContext)
    : IRequestHandler<GetAchievementsQuery, AchievementsDto>
{
    public async Task<AchievementsDto> Handle(GetAchievementsQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var today = DateOnly.FromDateTime(DateTime.Today);

        logger.LogInformation("Handling GetAchievementsQuery for user {UserId}", currentUser.Id);

        var tasksDone = await tasksRepository.GetTotalCompletedCountAsync(currentUser.Id, cancellationToken);
        var completionDates = await tasksRepository.GetAllCompletionDatesAsync(currentUser.Id, cancellationToken);
        var user = await usersRepository.GetWithStatsReadOnlyAsync(currentUser.Id, cancellationToken);

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

        var dtos = AchievementCatalog.All.Select(def => new AchievementDto
        {
            Key = def.Key,
            Label = def.Label,
            Subtitle = def.Subtitle,
            Emoji = def.Emoji,
            Variant = def.Variant,
            Locked = !def.IsUnlocked(context),
        }).ToArray();

        return new AchievementsDto
        {
            Achievements = dtos,
            UnlockedCount = dtos.Count(d => !d.Locked),
            TotalCount = dtos.Length,
        };
    }
}
