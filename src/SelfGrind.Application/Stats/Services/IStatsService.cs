using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Entities;

namespace SelfGrind.Application.Stats.Services;

public interface IStatsService
{
    void AwardUserExp(Domain.Entities.User user, int amount);
    void RevokeUserExp(Domain.Entities.User user, int amount);
    void AwardStatExp(UserStat stat, int amount);
    void RevokeStatExp(UserStat stat, int amount);

    /// <summary>Awards exp to the user and the matching attribute stat. Returns true if the user levelled up.</summary>
    bool AwardTaskExp(Domain.Entities.User user, BaseAttribute attribute, int amount);

    /// <summary>Revokes exp from the user and the matching attribute stat.</summary>
    void RevokeTaskExp(Domain.Entities.User user, BaseAttribute attribute, int amount);
}
