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
        var weeklyXpByUser = await dbContext.TaskOccurrences
            .AsNoTracking()
            .Where(o => o.Status == TaskOccurrenceStatus.Done
                     && o.CompletedDate != null
                     && o.CompletedDate >= weekStart
                     && !o.TaskItem.IsArchived)
            .GroupBy(o => o.TaskItem.UserId)
            .Select(g => new { UserId = g.Key, WeeklyExp = g.Sum(o => o.TaskItem.Exp) })
            .ToDictionaryAsync(x => x.UserId, x => x.WeeklyExp, cancellationToken);

        var users = await dbContext.Users
            .AsNoTracking()
            .Select(u => new { u.Id, u.UserName, u.Level, u.Exp })
            .ToArrayAsync(cancellationToken);

        return users
            .Select(u =>
            {
                weeklyXpByUser.TryGetValue(u.Id, out var weekly);
                return new LeaderboardRow(u.Id, u.UserName ?? "Anonymous", u.Level, u.Exp, weekly);
            })
            .OrderByDescending(r => r.WeeklyExp)
            .ThenByDescending(r => r.Level)
            .ThenByDescending(r => r.TotalExp)
            .Take(top)
            .ToArray();
    }


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
