using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.User;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Habits.Commands.CreateHabit;

public class CreateHabitCommandHandler(ILogger<CreateHabitCommandHandler> logger, IHabitsRepository habitsRepository, IUserContext userContext) : IRequestHandler<CreateHabitCommand, Guid>
{
    public async Task<Guid> Handle(CreateHabitCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Handling CreateHabitCommand for user {userId}", currentUser.Id);

        var habit = new Habit
        {
            Name = request.Name,
            TargetValue = request.TargetValue,
            Unit = request.Unit,
            UserId = currentUser.Id,
        };

        var id = await habitsRepository.Create(habit);
        return id;
    }
}