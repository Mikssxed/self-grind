using SelfGrind.Domain.Entities;

namespace SelfGrind.Domain.Repositories;

public interface IEvolutionTiersRepository
{
    Task<EvolutionTier[]> GetAllOrderedAsync(CancellationToken cancellationToken = default);
}
