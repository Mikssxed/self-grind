using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Character.Services;

public interface IUserItemGranter
{
    Task GrantUnlockedItemsAsync(string userId, int userLevel, CancellationToken cancellationToken = default);
}

/// <summary>
/// Orchestrates item granting after level changes: loads the catalog and the user's owned items,
/// computes newly unlocked items via <see cref="IItemGrantingService"/>, and persists the grants.
/// </summary>
public class UserItemGranter(
    IItemsRepository itemsRepository,
    IUserItemsRepository userItemsRepository,
    IItemGrantingService itemGrantingService) : IUserItemGranter
{
    public async Task GrantUnlockedItemsAsync(string userId, int userLevel, CancellationToken cancellationToken = default)
    {
        var catalog = await itemsRepository.GetAllAsync(cancellationToken);
        var owned = await userItemsRepository.GetForUserAsync(userId, cancellationToken);
        var newGrants = itemGrantingService.CalculateNewlyGranted(userId, catalog, owned, userLevel);
        if (newGrants.Count > 0)
        {
            await userItemsRepository.AddRangeAsync(newGrants, cancellationToken);
        }
    }
}
