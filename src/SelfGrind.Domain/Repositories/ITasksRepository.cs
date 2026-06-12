using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Entities;

namespace SelfGrind.Domain.Repositories;

public interface ITasksRepository
{
    Task<Guid> Create(TaskItem taskItem, CancellationToken cancellationToken = default);
    Task<Guid> AddLoggedActivityAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
    Task<(TaskItem[] Items, int TotalCount)> GetAllAsync(string userId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<TaskItem?> GetByIdAsync(string userId, Guid taskItemId, CancellationToken cancellationToken = default);
    Task<TaskOccurrence[]> GetTodayTasksAsync(string userId, DateOnly today, CancellationToken cancellationToken = default);
    Task UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
    Task ArchiveAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
    Task<TaskOccurrence?> GetTaskOccurenceById(string userId, Guid taskOccurenceId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<int> GetTodayTotalExp(string userId, DateOnly today, CancellationToken cancellationToken = default);
    Task<int> GetTotalCompletedCountAsync(string userId, CancellationToken cancellationToken = default);
    Task<(DateOnly Date, int Completed)[]> GetDailyCompletionSummaryAsync(string userId, DateOnly today, CancellationToken cancellationToken = default);
    Task<(DateOnly Date, int CompletedCount, int TotalCount)[]> GetYearlyCompletionSummaryAsync(string userId, int year, CancellationToken cancellationToken = default);
    Task<TaskOccurrence[]> GetCompletedOccurrencesForDateAsync(string userId, DateOnly date, CancellationToken cancellationToken = default);
    Task<int[]> GetActiveYearsAsync(string userId, CancellationToken cancellationToken = default);
    Task<(DateOnly Date, BaseAttribute Attribute, int CompletedCount, int TotalExp)[]> GetCompletedAggregatesAsync(string userId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
    Task<(BaseAttribute Attribute, int TotalExp)[]> GetCompletedExpByAttributeAsync(string userId, DateOnly beforeDate, CancellationToken cancellationToken = default);
    Task<int> GetTotalEarnedExpAsync(string userId, CancellationToken cancellationToken = default);
    Task<DateOnly[]> GetAllCompletionDatesAsync(string userId, CancellationToken cancellationToken = default);
}