using SelfGrind.Domain.Entities;

namespace SelfGrind.Domain.Repositories;

public interface IUserItemsRepository
{
    Task<UserItem[]> GetForUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<UserItem?> GetForUserAsync(string userId, Guid userItemId, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<UserItem> userItems, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
