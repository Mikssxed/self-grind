using SelfGrind.Domain.Exceptions;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Common;

public static class UsersRepositoryExtensions
{
    public static async Task<Domain.Entities.User> GetWithStatsOrThrowAsync(
        this IUsersRepository repository, string userId, CancellationToken cancellationToken = default)
    {
        return await repository.GetWithStatsAsync(userId, cancellationToken)
               ?? throw new NotFoundException("user", userId);
    }

    public static async Task<Domain.Entities.User> GetWithStatsReadOnlyOrThrowAsync(
        this IUsersRepository repository, string userId, CancellationToken cancellationToken = default)
    {
        return await repository.GetWithStatsReadOnlyAsync(userId, cancellationToken)
               ?? throw new NotFoundException("user", userId);
    }
}
