using SelfGrind.Domain.Entities;

namespace SelfGrind.Domain.Repositories;

public interface IItemsRepository
{
    Task<Item[]> GetAllAsync(CancellationToken cancellationToken = default);
}
