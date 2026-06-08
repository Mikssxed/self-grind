using SelfGrind.Application.Character.Constants;

namespace SelfGrind.Application.Character.Dtos;

public class EvolutionTierDto
{
    public required string Name { get; set; }
    public required string LevelRange { get; set; }
    public EvolutionTierStatus Status { get; set; }
    public required string Emoji { get; set; }
}
