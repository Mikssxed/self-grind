using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.User;
using SelfGrind.Domain.Exceptions;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Habits.Commands.DeleteHabit;

public class DeleteHabitCommandHandler(
    ILogger<DeleteHabitCommandHandler> logger,
    IHabitsRepository habitsRepository,
    IUserContext userContext) : IRequestHandler<DeleteHabitCommand>
{
    public async Task Handle(DeleteHabitCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Deleting habit {HabitId} for user {UserId}", request.Id, currentUser.Id);

        var habit = await habitsRepository.GetByIdAsync(currentUser.Id, request.Id, cancellationToken);
        if (habit is null)
            throw new NotFoundException("habit", request.Id.ToString());

        await habitsRepository.DeleteAsync(habit, cancellationToken);
    }
}
