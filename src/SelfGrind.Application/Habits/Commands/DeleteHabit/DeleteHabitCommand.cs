using MediatR;

namespace SelfGrind.Application.Habits.Commands.DeleteHabit;

public class DeleteHabitCommand(Guid id) : IRequest
{
    public Guid Id { get; } = id;
}
