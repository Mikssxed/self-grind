using SelfGrind.Domain.Entities;

namespace SelfGrind.Domain.Repositories;

public interface IUsersRepository
{
    Task<User?> GetWithStatsAsync(string userId, CancellationToken cancellationToken = default);
    Task SeedStatsAsync(string userId, CancellationToken cancellationToken = default);
}
