using SelfGrind.Application.Character.Constants;
using SelfGrind.Application.Character.Dtos;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Entities;

namespace SelfGrind.Application.Character.Services;

public class HeroStatCalculator : IHeroStatCalculator
{
    private const int PointsPerLevel = 5;
    private const int BasePoints = 10;

    private static readonly HeroStatDefinition[] Definitions =
    [
        new("⚔️", "Attack", BaseAttribute.Strength, HeroStatVariant.Default),
        new("🛡️", "Defense", BaseAttribute.Discipline, HeroStatVariant.Info),
        new("🧠", "Intelligence", BaseAttribute.Knowledge, HeroStatVariant.Violet),
        new("💚", "Vitality", BaseAttribute.Health, HeroStatVariant.Info),
    ];

    public HeroStatDto[] Calculate(IEnumerable<UserStat> stats)
    {
        var statsByAttribute = stats.ToDictionary(s => s.Attribute, s => s.Level);

        return Definitions
            .Select(def => new HeroStatDto
            {
                Emoji = def.Emoji,
                Label = def.Label,
                Value = FormatValue(LookupLevel(statsByAttribute, def.Attribute)),
                Variant = def.Variant
            })
            .ToArray();
    }

    private static int LookupLevel(IReadOnlyDictionary<BaseAttribute, int> statsByAttribute, BaseAttribute attribute)
        => statsByAttribute.TryGetValue(attribute, out var level) ? level : 1;

    private static string FormatValue(int attributeLevel)
        => $"+{BasePoints + (attributeLevel * PointsPerLevel)}";

    private record HeroStatDefinition(string Emoji, string Label, BaseAttribute Attribute, HeroStatVariant Variant);
}
