namespace SelfGrind.Application.Character.Dtos;

public class CharacterHeroDto
{
    public int Level { get; set; }
    public int Exp { get; set; }
    public int RequiredExp { get; set; }
    public required string StageName { get; set; }
    public string? NextEvolutionLabel { get; set; }
    public HeroStatDto[] Stats { get; set; } = [];
}
