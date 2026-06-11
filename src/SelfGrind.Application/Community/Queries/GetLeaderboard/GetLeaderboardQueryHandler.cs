using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Character.Services;
using SelfGrind.Application.Common;
using SelfGrind.Application.Community.Dtos;
using SelfGrind.Application.Community.Services;
using SelfGrind.Application.User;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Community.Queries.GetLeaderboard;

public class GetLeaderboardQueryHandler(
    ILogger<GetLeaderboardQueryHandler> logger,
    IUsersRepository usersRepository,
    IEvolutionTiersRepository tiersRepository,
    IEvolutionTierResolver tierResolver,
    IUserContext userContext) : IRequestHandler<GetLeaderboardQuery, LeaderboardDto>
{
    public async Task<LeaderboardDto> Handle(GetLeaderboardQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var weekStart = request.WeekStart ?? DateUtils.GetMondayOf(DateUtils.UtcToday);
        logger.LogInformation("Handling GetLeaderboardQuery weekStart={weekStart} top={top}", weekStart, request.Top);

        var rows = await usersRepository.GetLeaderboardAsync(request.Top, weekStart, cancellationToken);
        var tiers = await tiersRepository.GetAllOrderedAsync(cancellationToken);

        var entries = rows
            .Select((row, index) => new LeaderboardEntryDto
            {
                Rank = index + 1,
                Name = row.DisplayName,
                Level = row.Level,
                Title = tierResolver.ResolveCurrent(tiers, row.Level)?.StageName ?? "Adventurer",
                Xp = row.WeeklyExp,
                IsCurrentUser = row.UserId == currentUser.Id,
                AvatarEmoji = AvatarEmojiProvider.For(row.UserId),
            })
            .ToArray();

        return new LeaderboardDto
        {
            WeekStart = weekStart.ToString("O"),
            Entries = entries,
        };
    }
}
