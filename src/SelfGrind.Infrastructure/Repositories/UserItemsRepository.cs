using Microsoft.EntityFrameworkCore;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Repositories;
using SelfGrind.Infrastructure.Persistence;

namespace SelfGrind.Infrastructure.Repositories;

public class UserItemsRepository(SelfGrindDbContext dbContext) : IUserItemsRepository
{
    public async Task<UserItem[]> GetForUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.UserItems
            .Include(ui => ui.Item)
            .Where(ui => ui.UserId == userId)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<UserItem?> GetForUserAsync(string userId, Guid userItemId, CancellationToken cancellationToken = default)
    {
        return await dbContext.UserItems
            .Include(ui => ui.Item)
            .FirstOrDefaultAsync(ui => ui.UserId == userId && ui.Id == userItemId, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<UserItem> userItems, CancellationToken cancellationToken = default)
    {
        await dbContext.UserItems.AddRangeAsync(userItems, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
