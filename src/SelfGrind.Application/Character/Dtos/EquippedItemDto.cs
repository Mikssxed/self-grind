using SelfGrind.Domain.Constants;

namespace SelfGrind.Application.Character.Dtos;

public class EquippedItemDto
{
    public Guid UserItemId { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public required string Bonus { get; set; }
    public required string Emoji { get; set; }
    public ItemVariant Variant { get; set; }
}
