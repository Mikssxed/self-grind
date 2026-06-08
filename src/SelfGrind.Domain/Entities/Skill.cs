using SelfGrind.Domain.Constants;

namespace SelfGrind.Domain.Entities;

public class Skill
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Emoji { get; set; }
    public required string Description { get; set; }
    public BaseAttribute Attribute { get; set; }
    public int RequiredAttributeLevel { get; set; }
    public int Order { get; set; }
}
