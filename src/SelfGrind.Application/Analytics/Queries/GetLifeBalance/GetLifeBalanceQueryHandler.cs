using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Analytics.Dtos;
using SelfGrind.Application.User;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Exceptions;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Analytics.Queries.GetLifeBalance;

public class GetLifeBalanceQueryHandler(
    ILogger<GetLifeBalanceQueryHandler> logger,
    IUsersRepository usersRepository,
    IUserContext userContext)
    : IRequestHandler<GetLifeBalanceQuery, LifeBalanceDto>
{
    public async Task<LifeBalanceDto> Handle(GetLifeBalanceQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        logger.LogInformation("Handling GetLifeBalanceQuery for user {UserId}", currentUser.Id);

        var user = await usersRepository.GetWithStatsAsync(currentUser.Id, cancellationToken);
        if (user == null) throw new NotFoundException("user", currentUser.Id);

        var statsByAttribute = user.Stats.ToDictionary(s => s.Attribute);

        var entries = Enum.GetValues<BaseAttribute>()
            .Select(attr =>
            {
                if (statsByAttribute.TryGetValue(attr, out var stat))
                {
                    var progressInLevel = stat.RequiredExp > 0
                        ? (double)stat.Exp / stat.RequiredExp
                        : 0;
                    var score = Math.Min(100, (stat.Level - 1) * 5 + progressInLevel * 5);
                    return new LifeBalanceEntryDto
                    {
                        Attribute = attr,
                        Level = stat.Level,
                        Exp = stat.Exp,
                        RequiredExp = stat.RequiredExp,
                        Score = Math.Round(score, 1),
                    };
                }

                return new LifeBalanceEntryDto
                {
                    Attribute = attr,
                    Level = 1,
                    Exp = 0,
                    RequiredExp = 100,
                    Score = 0,
                };
            })
            .OrderBy(e => (int)e.Attribute)
            .ToArray();

        return new LifeBalanceDto { Attributes = entries };
    }
}
