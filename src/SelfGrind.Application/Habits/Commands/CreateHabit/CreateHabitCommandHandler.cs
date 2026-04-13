using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.User;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Habits.Commands.CreateHabit;

public class CreateHabitCommandHandler(ILogger<CreateHabitCommandHandler> logger, IMapper mapper, IHabitsRepository habitsRepository, IUserContext userContext) : IRequestHandler<CreateHabitCommand, Guid>
{
    public async Task<Guid> Handle(CreateHabitCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Handling CreateHabitCommand for user {userId}", currentUser.Id);
        
        var habit = mapper.Map<Habit>(request);
        habit.UserId = currentUser.Id;
        
        var id = await habitsRepository.Create(habit);
        return id;
    }
}