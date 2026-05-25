using SelfGrind.Domain.Entities;

namespace SelfGrind.Application.Stats.Services;

public interface IStatsService
{
    void AwardUserExp(Domain.Entities.User user, int amount);
    void RevokeUserExp(Domain.Entities.User user, int amount);
    void AwardStatExp(UserStat stat, int amount);
    void RevokeStatExp(UserStat stat, int amount);
}
