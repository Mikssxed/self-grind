using Microsoft.EntityFrameworkCore;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Repositories;
using SelfGrind.Infrastructure.Persistence;

namespace SelfGrind.Infrastructure.Repositories;

public class UsersRepository(SelfGrindDbContext dbContext) : IUsersRepository
{
    public async Task<LeaderboardRow[]> GetLeaderboardAsync(int top, DateOnly weekStart, CancellationToken cancellationToken = default)
    {
        var rows = await dbContext.Users
            .AsNoTracking()
            .Select(u => new
            {
                u.Id,
                u.UserName,
                u.Level,
                u.Exp,
                WeeklyExp = dbContext.TaskOccurrences
                    .Where(o => o.TaskItem.UserId == u.Id
                             && o.Status == TaskOccurrenceStatus.Done
                             && o.CompletedDate != null
                             && o.CompletedDate >= weekStart
                             && !o.TaskItem.IsArchived)
                    .Sum(o => (int?)o.TaskItem.Exp) ?? 0
            })
            .OrderByDescending(x => x.WeeklyExp)
            .ThenByDescending(x => x.Level)
            .ThenByDescending(x => x.Exp)
            .Take(top)
            .ToArrayAsync(cancellationToken);

        return rows
            .Select(r => new LeaderboardRow(r.Id, r.UserName ?? "Anonymous", r.Level, r.Exp, r.WeeklyExp))
            .ToArray();
    }

    public async Task<User?> GetWithStatsAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Include(u => u.Stats)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<User?> GetWithStatsReadOnlyAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
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
