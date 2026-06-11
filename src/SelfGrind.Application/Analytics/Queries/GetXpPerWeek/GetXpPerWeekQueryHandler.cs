using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Analytics.Dtos;
using SelfGrind.Application.Common;
using SelfGrind.Application.User;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Analytics.Queries.GetXpPerWeek;

public class GetXpPerWeekQueryHandler(
    ILogger<GetXpPerWeekQueryHandler> logger,
    ITasksRepository tasksRepository,
    IUserContext userContext)
    : IRequestHandler<GetXpPerWeekQuery, XpPerWeekDto>
{
    public async Task<XpPerWeekDto> Handle(GetXpPerWeekQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var today = DateUtils.LocalToday;
        var currentMonday = DateUtils.GetMondayOf(today);
        var earliestMonday = currentMonday.AddDays(-7 * (request.Weeks - 1));
        var windowEnd = currentMonday.AddDays(6);

        logger.LogInformation(
            "Handling GetXpPerWeekQuery for user {UserId}, weeks {Weeks}",
            currentUser.Id, request.Weeks);

        var aggregates = await tasksRepository.GetCompletedAggregatesAsync(currentUser.Id, earliestMonday, windowEnd, cancellationToken);

        var weeks = new XpPerWeekEntryDto[request.Weeks];
        for (var i = 0; i < request.Weeks; i++)
        {
            var weekStart = earliestMonday.AddDays(i * 7);
            var weekEnd = weekStart.AddDays(6);
            var xp = aggregates
                .Where(a => a.Date >= weekStart && a.Date <= weekEnd)
                .Sum(a => a.TotalExp);

            weeks[i] = new XpPerWeekEntryDto
            {
                WeekStart = weekStart,
                WeekNumber = i + 1,
                Xp = xp,
            };
        }

        return new XpPerWeekDto { Weeks = weeks };
    }
}
