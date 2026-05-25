using Microsoft.EntityFrameworkCore;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Repositories;
using SelfGrind.Infrastructure.Persistence;

namespace SelfGrind.Infrastructure.Repositories;

public class UsersRepository(SelfGrindDbContext dbContext) : IUsersRepository
{
    public async Task<User?> GetWithStatsAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Include(u => u.Stats)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task SeedStatsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
            .Include(u => u.Stats)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null) return;

        var existing = user.Stats.Select(s => s.Attribute).ToHashSet();
        foreach (var attribute in Enum.GetValues<BaseAttribute>())
        {
            if (existing.Contains(attribute)) continue;
            user.Stats.Add(new UserStat
            {
                UserId = userId,
                User = user,
                Attribute = attribute,
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
