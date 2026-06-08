using SelfGrind.Domain.Constants;

namespace SelfGrind.Application.Character.Dtos;

public class InventoryItemDto
{
    public Guid UserItemId { get; set; }
    public required string Name { get; set; }
    public required string Bonus { get; set; }
    public required string Emoji { get; set; }
    public ItemRarity Rarity { get; set; }
    public bool IsEquipped { get; set; }
}
