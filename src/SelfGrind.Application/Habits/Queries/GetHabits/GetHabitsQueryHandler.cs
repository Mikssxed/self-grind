using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Common;
using SelfGrind.Application.Tasks.Dtos;
using SelfGrind.Application.User;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Habits.Queries.GetHabits;

public class GetHabitsQueryHandler(ILogger<GetHabitsQueryHandler> logger, IHabitsRepository habitsRepository, IUserContext userContext) :
    IRequestHandler<GetHabitsQuery, PagedResult<HabitDto>>
{
    public async Task<PagedResult<HabitDto>> Handle(GetHabitsQuery request, CancellationToken cancellationToken)
    {
        var userId = userContext.GetCurrentUser().Id;
        logger.LogInformation("Handling GetHabitsQuery");

        var today = DateTime.UtcNow.Date;
        var (habits, totalCount) = await habitsRepository.GetAllWithEntriesForDayAsync(userId, today, request.Page, request.PageSize, cancellationToken);

        return new PagedResult<HabitDto>
        {
            Items = habits.Select(h => new HabitDto
            {
                Id = h.Id,
                Name = h.Name,
                TargetValue = h.TargetValue,
                Unit = h.Unit,
                TodayValue = h.HabitEntries?.FirstOrDefault()?.Value ?? 0,
            }).ToArray(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
        };
    }
}
