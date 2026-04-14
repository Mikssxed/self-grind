using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.User;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Exceptions;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Habits.Commands.LogHabitEntry;

public class LogHabitEntryCommandHandler(
    ILogger<LogHabitEntryCommandHandler> logger,
    IHabitsRepository habitsRepository,
    IUserContext userContext) : IRequestHandler<LogHabitEntryCommand>
{
    public async Task Handle(LogHabitEntryCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Logging entry for habit {HabitId} for user {UserId}", request.HabitId, currentUser.Id);

        var habit = await habitsRepository.GetByIdAsync(currentUser.Id, request.HabitId, cancellationToken);
        if (habit is null)
            throw new NotFoundException("habit", request.HabitId.ToString());

        var entry = new HabitEntry
        {
            HabitId = habit.Id,
            EntryDate = DateTime.UtcNow.Date,
            Value = request.Value
        };

        await habitsRepository.UpsertEntryAsync(entry, cancellationToken);
    }
}
