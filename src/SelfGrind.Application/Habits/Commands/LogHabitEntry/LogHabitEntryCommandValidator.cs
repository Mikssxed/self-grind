using FluentValidation;

namespace SelfGrind.Application.Habits.Commands.LogHabitEntry;

public class LogHabitEntryCommandValidator : AbstractValidator<LogHabitEntryCommand>
{
    public LogHabitEntryCommandValidator()
    {
        RuleFor(c => c.HabitId).NotEmpty();
        RuleFor(c => c.Value).GreaterThanOrEqualTo(0);
    }
}
