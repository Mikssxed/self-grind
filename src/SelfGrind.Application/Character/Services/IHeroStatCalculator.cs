using SelfGrind.Application.Character.Dtos;
using SelfGrind.Domain.Entities;

namespace SelfGrind.Application.Character.Services;

public interface IHeroStatCalculator
{
    HeroStatDto[] Calculate(IEnumerable<UserStat> stats);
}
