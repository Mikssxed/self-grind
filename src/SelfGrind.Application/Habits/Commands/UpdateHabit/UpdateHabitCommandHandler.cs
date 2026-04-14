using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.User;
using SelfGrind.Domain.Exceptions;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Habits.Commands.UpdateHabit;

public class UpdateHabitCommandHandler(
    ILogger<UpdateHabitCommandHandler> logger,
    IHabitsRepository habitsRepository,
    IUserContext userContext) : IRequestHandler<UpdateHabitCommand>
{
    public async Task Handle(UpdateHabitCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Updating habit {HabitId} for user {UserId}", request.Id, currentUser.Id);

        var habit = await habitsRepository.GetByIdAsync(currentUser.Id, request.Id, cancellationToken);
        if (habit is null)
            throw new NotFoundException("habit", request.Id.ToString());

        habit.Name = request.Name;
        habit.TargetValue = request.TargetValue;
        habit.Unit = request.Unit;
        habit.UpdatedAt = DateTime.UtcNow;

        await habitsRepository.UpdateAsync(habit, cancellationToken);
    }
}
