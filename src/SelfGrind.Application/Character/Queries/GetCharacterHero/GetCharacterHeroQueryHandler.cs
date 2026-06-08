using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Character.Dtos;
using SelfGrind.Application.Character.Services;
using SelfGrind.Application.User;
using SelfGrind.Domain.Exceptions;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Character.Queries.GetCharacterHero;

public class GetCharacterHeroQueryHandler(
    ILogger<GetCharacterHeroQueryHandler> logger,
    IUsersRepository usersRepository,
    IEvolutionTiersRepository tiersRepository,
    IEvolutionTierResolver tierResolver,
    IHeroStatCalculator heroStatCalculator,
    IUserContext userContext) : IRequestHandler<GetCharacterHeroQuery, CharacterHeroDto>
{
    public async Task<CharacterHeroDto> Handle(GetCharacterHeroQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Handling GetCharacterHeroQuery for user {userId}", currentUser.Id);

        var user = await usersRepository.GetWithStatsAsync(currentUser.Id, cancellationToken);
        if (user is null) throw new NotFoundException("user", currentUser.Id);

        var tiers = await tiersRepository.GetAllOrderedAsync(cancellationToken);
        var currentTier = tierResolver.ResolveCurrent(tiers, user.Level);

        return new CharacterHeroDto
        {
            Level = user.Level,
            Exp = user.Exp,
            RequiredExp = user.RequiredExp,
            StageName = currentTier?.StageName ?? "Adventurer",
            NextEvolutionLabel = currentTier?.NextEvolutionLabel,
            Stats = heroStatCalculator.Calculate(user.Stats)
        };
    }
}
