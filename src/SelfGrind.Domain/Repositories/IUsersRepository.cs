using SelfGrind.Domain.Entities;

namespace SelfGrind.Domain.Repositories;

public record LeaderboardRow(string UserId, string DisplayName, int Level, int TotalExp, int WeeklyExp);

public interface IUsersRepository
{
    Task<User?> GetWithStatsAsync(string userId, CancellationToken cancellationToken = default);
    Task SeedStatsAsync(string userId, CancellationToken cancellationToken = default);
    Task<LeaderboardRow[]> GetLeaderboardAsync(int top, DateOnly weekStart, CancellationToken cancellationToken = default);
}
