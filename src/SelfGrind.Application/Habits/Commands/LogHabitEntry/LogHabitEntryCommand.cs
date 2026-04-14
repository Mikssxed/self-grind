using MediatR;

namespace SelfGrind.Application.Habits.Commands.LogHabitEntry;

public class LogHabitEntryCommand : IRequest
{
    public Guid HabitId { get; set; }
    public int Value { get; set; }
}
