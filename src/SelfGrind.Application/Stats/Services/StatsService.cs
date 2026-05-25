using Microsoft.Extensions.Options;
using SelfGrind.Application.Settings;
using SelfGrind.Domain.Entities;

namespace SelfGrind.Application.Stats.Services;

public class StatsService(IOptions<StatsSettings> options) : IStatsService
{
    private readonly StatsSettings _settings = options.Value;

    public void AwardUserExp(Domain.Entities.User user, int amount)
    {
        if (amount <= 0) return;
        var (level, exp, required) = ApplyAward(user.Level, user.Exp, user.RequiredExp, amount, _settings.LevelUpExpMultiplier);
        user.Level = level;
        user.Exp = exp;
        user.RequiredExp = required;
    }

    public void RevokeUserExp(Domain.Entities.User user, int amount)
    {
        if (amount <= 0) return;
        var (level, exp, required) = ApplyRevoke(user.Level, user.Exp, user.RequiredExp, amount, _settings.LevelUpExpMultiplier);
        user.Level = level;
        user.Exp = exp;
        user.RequiredExp = required;
    }

    public void AwardStatExp(UserStat stat, int amount)
    {
        if (amount <= 0) return;
        var (level, exp, required) = ApplyAward(stat.Level, stat.Exp, stat.RequiredExp, amount, _settings.StatLevelUpExpMultiplier);
        stat.Level = level;
        stat.Exp = exp;
        stat.RequiredExp = required;
    }

    public void RevokeStatExp(UserStat stat, int amount)
    {
        if (amount <= 0) return;
        var (level, exp, required) = ApplyRevoke(stat.Level, stat.Exp, stat.RequiredExp, amount, _settings.StatLevelUpExpMultiplier);
        stat.Level = level;
        stat.Exp = exp;
        stat.RequiredExp = required;
    }

    private static (int level, int exp, int required) ApplyAward(int level, int exp, int required, int amount, double multiplier)
    {
        exp += amount;
        while (exp >= required)
        {
            exp -= required;
            level++;
            required = (int)Math.Ceiling(required * multiplier);
        }
        return (level, exp, required);
    }

    private static (int level, int exp, int required) ApplyRevoke(int level, int exp, int required, int amount, double multiplier)
    {
        exp -= amount;
        while (exp < 0 && level > 1)
        {
            level--;
            required = (int)Math.Ceiling(required / multiplier);
            exp += required;
        }
        if (exp < 0) exp = 0;
        return (level, exp, required);
    }
}
