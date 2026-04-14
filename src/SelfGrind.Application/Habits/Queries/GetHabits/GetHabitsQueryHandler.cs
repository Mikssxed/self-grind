using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Tasks.Dtos;
using SelfGrind.Application.User;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Habits.Queries.GetHabits;

public class GetHabitsQueryHandler(ILogger<GetHabitsQueryHandler> logger, IHabitsRepository habitsRepository, IUserContext userContext) :
    IRequestHandler<GetHabitsQuery, HabitDto[]>
{
    public async Task<HabitDto[]> Handle(GetHabitsQuery request, CancellationToken cancellationToken)
    {
        var userId = userContext.GetCurrentUser().Id;
        logger.LogInformation("Handling GetHabitsQuery");

        var today = DateTime.UtcNow.Date;
        var habits = await habitsRepository.GetAllAsync(userId, cancellationToken);

        return habits.Select(h => new HabitDto
        {
            Id = h.Id,
            Name = h.Name,
            TargetValue = h.TargetValue,
            Unit = h.Unit,
            TodayValue = h.HabitEntries?.FirstOrDefault(e => e.EntryDate.Date == today)?.Value ?? 0,
        }).ToArray();
    }
}