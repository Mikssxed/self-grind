using SelfGrind.Application.Character.Constants;
using SelfGrind.Domain.Constants;

namespace SelfGrind.Application.Character.Dtos;

public class SkillCategoryDto
{
    public BaseAttribute Attribute { get; set; }
    public required string Name { get; set; }
    public required string Emoji { get; set; }
    public SkillCategoryVariant Variant { get; set; }
    public SkillDto[] Skills { get; set; } = [];
}
