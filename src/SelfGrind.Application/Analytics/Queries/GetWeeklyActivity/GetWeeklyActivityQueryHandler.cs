using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Analytics.Dtos;
using SelfGrind.Application.User;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Analytics.Queries.GetWeeklyActivity;

public class GetWeeklyActivityQueryHandler(
    ILogger<GetWeeklyActivityQueryHandler> logger,
    ITasksRepository tasksRepository,
    IUserContext userContext)
    : IRequestHandler<GetWeeklyActivityQuery, WeeklyActivityDto>
{
    public async Task<WeeklyActivityDto> Handle(GetWeeklyActivityQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var today = DateOnly.FromDateTime(DateTime.Today);
        var weekStart = request.WeekStart ?? GetMondayOf(today);
        var weekEnd = weekStart.AddDays(6);

        logger.LogInformation(
            "Handling GetWeeklyActivityQuery for user {UserId}, week starting {WeekStart}",
            currentUser.Id, weekStart);

        var aggregates = await tasksRepository.GetCompletedAggregatesAsync(currentUser.Id, weekStart, weekEnd, cancellationToken);

        var allAttributes = Enum.GetValues<BaseAttribute>();
        var byDateAttribute = aggregates.ToDictionary(a => (a.Date, a.Attribute), a => a.CompletedCount);

        var days = new WeeklyActivityDayDto[7];
        for (var i = 0; i < 7; i++)
        {
            var date = weekStart.AddDays(i);
            var counts = allAttributes.Select(attr => new AttributeCountDto
            {
                Attribute = attr,
                Count = byDateAttribute.TryGetValue((date, attr), out var n) ? n : 0,
            }).ToArray();

            days[i] = new WeeklyActivityDayDto
            {
                Date = date,
                DayOfWeek = date.DayOfWeek,
                Counts = counts,
            };
        }

        return new WeeklyActivityDto
        {
            WeekStart = weekStart,
            Days = days,
        };
    }

    private static DateOnly GetMondayOf(DateOnly date)
    {
        var diff = ((int)date.DayOfWeek + 6) % 7; // Monday = 0
        return date.AddDays(-diff);
    }
}
