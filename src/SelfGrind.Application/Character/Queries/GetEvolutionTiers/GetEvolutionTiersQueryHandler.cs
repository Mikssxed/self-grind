using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Character.Dtos;
using SelfGrind.Application.Character.Services;
using SelfGrind.Application.Common;
using SelfGrind.Application.User;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Character.Queries.GetEvolutionTiers;

public class GetEvolutionTiersQueryHandler(
    ILogger<GetEvolutionTiersQueryHandler> logger,
    IUsersRepository usersRepository,
    IEvolutionTiersRepository tiersRepository,
    IEvolutionTierResolver tierResolver,
    IUserContext userContext) : IRequestHandler<GetEvolutionTiersQuery, EvolutionTierDto[]>
{
    public async Task<EvolutionTierDto[]> Handle(GetEvolutionTiersQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Handling GetEvolutionTiersQuery for user {userId}", currentUser.Id);

        var user = await usersRepository.GetWithStatsReadOnlyOrThrowAsync(currentUser.Id, cancellationToken);

        var tiers = await tiersRepository.GetAllOrderedAsync(cancellationToken);

        return tiers
            .Select(tier => new EvolutionTierDto
            {
                Name = tier.Name,
                LevelRange = $"Level {tier.MinLevel}-{tier.MaxLevel}",
                Status = tierResolver.ResolveStatus(tier, user.Level),
                Emoji = tier.Emoji
            })
            .ToArray();
    }
}
