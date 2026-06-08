using SelfGrind.Domain.Entities;

namespace SelfGrind.Domain.Repositories;

public interface ISkillsRepository
{
    Task<Skill[]> GetAllOrderedAsync(CancellationToken cancellationToken = default);
}
