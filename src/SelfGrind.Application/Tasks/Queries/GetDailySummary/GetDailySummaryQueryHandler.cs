using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Tasks.Dtos;
using SelfGrind.Application.User;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Tasks.Queries.GetDailySummary;

public class GetDailySummaryQueryHandler(ILogger<GetDailySummaryQueryHandler> logger,
    ITasksRepository tasksRepository,
    IUserContext userContext)
    : IRequestHandler<GetDailySummaryQuery, DailySummaryDto>
{
    public async Task<DailySummaryDto> Handle(GetDailySummaryQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var today = DateOnly.FromDateTime(DateTime.Today);

        logger.LogInformation("Handling GetDailySummaryQuery for user {userId}", currentUser.Id);

        var totalExpEarnedToday = await tasksRepository.GetTodayTotalExp(currentUser.Id, today, cancellationToken);
        var completedCount = await tasksRepository.GetTotalCompletedCountAsync(currentUser.Id, cancellationToken);

        var dailySummary = await tasksRepository.GetDailyCompletionSummaryAsync(currentUser.Id, today, cancellationToken);

        var streak = 0;
        var expectedDate = today.AddDays(-1);
        foreach (var (date, completed) in dailySummary)
        {
            if (date != expectedDate || completed == 0) break;
            streak++;
            expectedDate = expectedDate.AddDays(-1);
        }

        return new DailySummaryDto
        {
            CompletedCount = completedCount,
            TotalExpEarnedToday = totalExpEarnedToday,
            Streak = streak
        };
    }
}
