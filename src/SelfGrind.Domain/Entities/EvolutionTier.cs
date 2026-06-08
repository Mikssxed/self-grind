namespace SelfGrind.Domain.Entities;

public class EvolutionTier
{
    public Guid Id { get; set; }
    public int Order { get; set; }
    public required string Name { get; set; }
    public required string Emoji { get; set; }
    public int MinLevel { get; set; }
    public int MaxLevel { get; set; }
    public required string StageName { get; set; }
    public string? NextEvolutionLabel { get; set; }
}
