using Microsoft.EntityFrameworkCore;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Repositories;
using SelfGrind.Infrastructure.Persistence;

namespace SelfGrind.Infrastructure.Repositories;

public class SkillsRepository(SelfGrindDbContext dbContext) : ISkillsRepository
{
    public async Task<Skill[]> GetAllOrderedAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Skills
            .OrderBy(s => s.Attribute)
            .ThenBy(s => s.Order)
            .ToArrayAsync(cancellationToken);
    }
}
