using MediatR;

namespace SelfGrind.Application.Habits.Commands.CreateHabit;

public class CreateHabitCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public int TargetValue { get; set; }
    public string? Unit { get; set; }
}