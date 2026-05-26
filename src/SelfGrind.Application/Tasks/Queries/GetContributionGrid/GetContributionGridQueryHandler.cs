using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Tasks.Dtos;
using SelfGrind.Application.User;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Tasks.Queries.GetContributionGrid;

public class GetContributionGridQueryHandler(
    ILogger<GetContributionGridQueryHandler> logger,
    ITasksRepository tasksRepository,
    IUserContext userContext)
    : IRequestHandler<GetContributionGridQuery, ContributionGridDto>
{
    public async Task<ContributionGridDto> Handle(GetContributionGridQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var today = DateOnly.FromDateTime(DateTime.Today);

        logger.LogInformation("Handling GetContributionGridQuery for user {UserId}, year {Year}", currentUser.Id, request.Year);

        var yearlySummary = await tasksRepository.GetYearlyCompletionSummaryAsync(currentUser.Id, request.Year, cancellationToken);
        var activeYears = await tasksRepository.GetActiveYearsAsync(currentUser.Id, cancellationToken);
        var allDailyHistory = await tasksRepository.GetDailyCompletionSummaryAsync(currentUser.Id, today, cancellationToken);

        var summaryByDate = yearlySummary.ToDictionary(s => s.Date);

        var days = BuildDays(request.Year, today, summaryByDate);
        var currentStreak = ComputeCurrentStreak(today, summaryByDate, allDailyHistory);
        var longestStreak = ComputeLongestStreak(request.Year, today, summaryByDate);
        var totalDaysActive = summaryByDate.Values.Count(s => s.CompletedCount > 0);

        var daysElapsed = request.Year == today.Year
            ? today.DayNumber - new DateOnly(request.Year, 1, 1).DayNumber + 1
            : DateTime.IsLeapYear(request.Year) ? 366 : 365;

        var activePercentage = daysElapsed > 0
            ? Math.Round((double)totalDaysActive / daysElapsed * 100, 1)
            : 0;

        var totalCompleted = yearlySummary.Sum(s => s.CompletedCount);
        var totalScheduled = yearlySummary.Sum(s => s.TotalCount);
        var completionRate = totalScheduled > 0
            ? Math.Round((double)totalCompleted / totalScheduled * 100, 1)
            : 0;

        return new ContributionGridDto
        {
            Days = days,
            CurrentStreak = currentStreak,
            LongestStreak = longestStreak,
            TotalDaysActive = totalDaysActive,
            ActivePercentage = activePercentage,
            CompletionRate = completionRate,
            AvailableYears = activeYears
        };
    }

    private static ContributionDayDto[] BuildDays(
        int year,
        DateOnly today,
        Dictionary<DateOnly, (DateOnly Date, int CompletedCount, int TotalCount)> summaryByDate)
    {
        var startDate = new DateOnly(year, 1, 1);
        var endDate = new DateOnly(year, 12, 31);
        var totalDays = endDate.DayNumber - startDate.DayNumber + 1;
        var days = new ContributionDayDto[totalDays];

        for (var i = 0; i < totalDays; i++)
        {
            var date = startDate.AddDays(i);
            var level = 0;

            if (date <= today && summaryByDate.TryGetValue(date, out var summary))
            {
                level = GetActivityLevel(summary.CompletedCount);
            }

            days[i] = new ContributionDayDto { Date = date, Level = level };
        }

        return days;
    }

    private static int ComputeCurrentStreak(
        DateOnly today,
        Dictionary<DateOnly, (DateOnly Date, int CompletedCount, int TotalCount)> currentYearSummary,
        (DateOnly Date, int Completed)[] allDailyHistory)
    {
        var streak = 0;

        if (currentYearSummary.TryGetValue(today, out var todaySummary) && todaySummary.CompletedCount > 0)
            streak = 1;

        // allDailyHistory is ordered descending, contains dates < today
        var expectedDate = today.AddDays(-1);
        foreach (var (date, completed) in allDailyHistory)
        {
            if (date != expectedDate || completed == 0) break;
            streak++;
            expectedDate = expectedDate.AddDays(-1);
        }

        return streak;
    }

    private static int ComputeLongestStreak(
        int year,
        DateOnly today,
        Dictionary<DateOnly, (DateOnly Date, int CompletedCount, int TotalCount)> summaryByDate)
    {
        var startDate = new DateOnly(year, 1, 1);
        var endDate = year == today.Year ? today : new DateOnly(year, 12, 31);

        var longestStreak = 0;
        var currentRun = 0;

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            if (summaryByDate.TryGetValue(date, out var summary) && summary.CompletedCount > 0)
            {
                currentRun++;
                if (currentRun > longestStreak)
                    longestStreak = currentRun;
            }
            else
            {
                currentRun = 0;
            }
        }

        return longestStreak;
    }

    private static int GetActivityLevel(int completedCount) => completedCount switch
    {
        0 => 0,
        1 => 1,
        2 or 3 => 2,
        4 or 5 => 3,
        _ => 4
    };
}
