using SelfGrind.Domain.Constants;

namespace SelfGrind.Domain.Entities;

public class Item
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Emoji { get; set; }
    public required string Type { get; set; }
    public required string Bonus { get; set; }
    public ItemRarity Rarity { get; set; }
    public ItemVariant Variant { get; set; }
    public int UnlockLevel { get; set; } = 1;
}
