namespace SelfGrind.Domain.Entities;

public class UserItem
{
    public Guid Id { get; set; }
    public required string UserId { get; set; }
    public Guid ItemId { get; set; }
    public bool IsEquipped { get; set; }
    public DateTimeOffset AcquiredAt { get; set; } = DateTimeOffset.UtcNow;

    public Item Item { get; set; } = null!;
    public User User { get; set; } = null!;
}
