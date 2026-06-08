using SelfGrind.Application.Character.Constants;

namespace SelfGrind.Application.Character.Dtos;

public class HeroStatDto
{
    public required string Emoji { get; set; }
    public required string Label { get; set; }
    public required string Value { get; set; }
    public HeroStatVariant Variant { get; set; }
}
