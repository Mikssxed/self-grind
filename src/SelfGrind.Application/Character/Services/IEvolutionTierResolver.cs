using SelfGrind.Application.Character.Constants;
using SelfGrind.Domain.Entities;

namespace SelfGrind.Application.Character.Services;

public interface IEvolutionTierResolver
{
    EvolutionTier? ResolveCurrent(IReadOnlyList<EvolutionTier> tiers, int userLevel);
    EvolutionTierStatus ResolveStatus(EvolutionTier tier, int userLevel);
}
