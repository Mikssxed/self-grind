using Microsoft.EntityFrameworkCore;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Repositories;
using SelfGrind.Infrastructure.Persistence;

namespace SelfGrind.Infrastructure.Repositories;

public class EvolutionTiersRepository(SelfGrindDbContext dbContext) : IEvolutionTiersRepository
{
    public async Task<EvolutionTier[]> GetAllOrderedAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.EvolutionTiers
            .OrderBy(t => t.Order)
            .ToArrayAsync(cancellationToken);
    }
}
