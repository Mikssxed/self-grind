using SelfGrind.Application.Character.Constants;
using SelfGrind.Domain.Constants;

namespace SelfGrind.Application.Character.Services;

public static class SkillCategoryMetadata
{
    private static readonly IReadOnlyDictionary<BaseAttribute, (string Emoji, SkillCategoryVariant Variant)> Map =
        new Dictionary<BaseAttribute, (string Emoji, SkillCategoryVariant Variant)>
        {
            [BaseAttribute.Strength] = ("💪", SkillCategoryVariant.Error),
            [BaseAttribute.Knowledge] = ("📘", SkillCategoryVariant.Info),
            [BaseAttribute.Focus] = ("👁️", SkillCategoryVariant.Success),
            [BaseAttribute.Discipline] = ("🧘", SkillCategoryVariant.Violet),
            [BaseAttribute.Health] = ("💚", SkillCategoryVariant.Success),
            [BaseAttribute.Energy] = ("⚡", SkillCategoryVariant.Warning),
        };

    public static (string Emoji, SkillCategoryVariant Variant) For(BaseAttribute attribute)
        => Map.TryGetValue(attribute, out var meta) ? meta : ("✨", SkillCategoryVariant.Info);
}
