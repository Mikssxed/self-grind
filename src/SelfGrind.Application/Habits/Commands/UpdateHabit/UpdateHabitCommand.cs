using MediatR;

namespace SelfGrind.Application.Habits.Commands.UpdateHabit;

public class UpdateHabitCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public int TargetValue { get; set; }
    public string? Unit { get; set; }
}
