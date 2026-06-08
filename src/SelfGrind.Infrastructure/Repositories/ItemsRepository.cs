using Microsoft.EntityFrameworkCore;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Repositories;
using SelfGrind.Infrastructure.Persistence;

namespace SelfGrind.Infrastructure.Repositories;

public class ItemsRepository(SelfGrindDbContext dbContext) : IItemsRepository
{
    public async Task<Item[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Items.ToArrayAsync(cancellationToken);
    }
}
