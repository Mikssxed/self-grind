using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Analytics.Dtos;
using SelfGrind.Application.Common;
using SelfGrind.Application.Stats.Services;
using SelfGrind.Application.User;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Analytics.Queries.GetStatGrowth;

public class GetStatGrowthQueryHandler(
    ILogger<GetStatGrowthQueryHandler> logger,
    ITasksRepository tasksRepository,
    IStatsService statsService,
    IUserContext userContext)
    : IRequestHandler<GetStatGrowthQuery, StatGrowthDto>
{
    public async Task<StatGrowthDto> Handle(GetStatGrowthQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var today = DateUtils.LocalToday;
        var currentMonday = DateUtils.GetMondayOf(today);

        logger.LogInformation(
            "Handling GetStatGrowthQuery for user {UserId}, weeks {Weeks}",
            currentUser.Id, request.Weeks);

        var weekEnds = new DateOnly[request.Weeks];
        var weekStarts = new DateOnly[request.Weeks];
        for (var i = 0; i < request.Weeks; i++)
        {
            var weekStart = currentMonday.AddDays(-7 * (request.Weeks - 1 - i));
            weekStarts[i] = weekStart;
            weekEnds[i] = weekStart.AddDays(6);
        }

        // Exp earned before the visible window is summed in SQL and awarded as one lump;
        // ApplyAward levels up in a loop, so this is equivalent to replaying each day individually.
        var firstWeekStart = weekStarts[0];
        var preWindowTotals = await tasksRepository.GetCompletedExpByAttributeAsync(
            currentUser.Id, firstWeekStart, cancellationToken);
        var aggregates = await tasksRepository.GetCompletedAggregatesAsync(
            currentUser.Id, firstWeekStart, today, cancellationToken);

        var attributes = Enum.GetValues<BaseAttribute>();
        var series = new StatGrowthSeriesDto[attributes.Length];

        for (var attrIndex = 0; attrIndex < attributes.Length; attrIndex++)
        {
            var attribute = attributes[attrIndex];
            var attrAggregates = aggregates
                .Where(a => a.Attribute == attribute)
                .OrderBy(a => a.Date)
                .ToArray();

            var stat = NewBlankStat(currentUser.Id, attribute);
            var preWindowExp = preWindowTotals.FirstOrDefault(t => t.Attribute == attribute).TotalExp;
            statsService.AwardStatExp(stat, preWindowExp);
            var levels = new int[request.Weeks];
            var weekIndex = 0;
            var cursor = 0;

            while (weekIndex < request.Weeks)
            {
                var weekEnd = weekEnds[weekIndex];
                while (cursor < attrAggregates.Length && attrAggregates[cursor].Date <= weekEnd)
                {
                    statsService.AwardStatExp(stat, attrAggregates[cursor].TotalExp);
                    cursor++;
                }

                levels[weekIndex] = stat.Level;
                weekIndex++;
            }

            series[attrIndex] = new StatGrowthSeriesDto
            {
                Attribute = attribute,
                Levels = levels,
            };
        }

        return new StatGrowthDto
        {
            WeekStarts = weekStarts,
            Series = series,
        };
    }

    private static Domain.Entities.UserStat NewBlankStat(string userId, BaseAttribute attribute)
    {
        var placeholderUser = new Domain.Entities.User { Id = userId };
        return new Domain.Entities.UserStat
        {
            UserId = userId,
            Attribute = attribute,
            User = placeholderUser,
        };
    }
}
