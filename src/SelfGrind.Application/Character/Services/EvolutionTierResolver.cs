using SelfGrind.Application.Character.Constants;
using SelfGrind.Domain.Entities;

namespace SelfGrind.Application.Character.Services;

public class EvolutionTierResolver : IEvolutionTierResolver
{
    public EvolutionTier? ResolveCurrent(IReadOnlyList<EvolutionTier> tiers, int userLevel)
    {
        var match = tiers.FirstOrDefault(t => userLevel >= t.MinLevel && userLevel <= t.MaxLevel);
        if (match is not null) return match;

        var topTier = tiers.OrderByDescending(t => t.MaxLevel).FirstOrDefault();
        return topTier is not null && userLevel > topTier.MaxLevel ? topTier : tiers.FirstOrDefault();
    }

    public EvolutionTierStatus ResolveStatus(EvolutionTier tier, int userLevel)
    {
        if (userLevel > tier.MaxLevel) return EvolutionTierStatus.Completed;
        if (userLevel >= tier.MinLevel) return EvolutionTierStatus.Current;
        return EvolutionTierStatus.Locked;
    }
}
