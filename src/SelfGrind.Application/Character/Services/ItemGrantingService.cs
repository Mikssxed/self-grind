using SelfGrind.Domain.Entities;

namespace SelfGrind.Application.Character.Services;

public class ItemGrantingService : IItemGrantingService
{
    public IReadOnlyList<UserItem> CalculateNewlyGranted(
        string userId,
        IEnumerable<Item> catalog,
        IEnumerable<UserItem> alreadyOwned,
        int userLevel)
    {
        var ownedItemIds = alreadyOwned.Select(ui => ui.ItemId).ToHashSet();

        return catalog
            .Where(item => item.UnlockLevel <= userLevel && !ownedItemIds.Contains(item.Id))
            .Select(item => new UserItem
            {
                UserId = userId,
                ItemId = item.Id,
                IsEquipped = false,
            })
            .ToArray();
    }
}
