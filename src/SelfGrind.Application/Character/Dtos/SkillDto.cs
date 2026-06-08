using SelfGrind.Application.Character.Constants;

namespace SelfGrind.Application.Character.Dtos;

public class SkillDto
{
    public required string Name { get; set; }
    public required string Emoji { get; set; }
    public required string Description { get; set; }
    public SkillStatus Status { get; set; }
}
