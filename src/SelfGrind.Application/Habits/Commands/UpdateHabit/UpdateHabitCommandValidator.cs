using FluentValidation;

namespace SelfGrind.Application.Habits.Commands.UpdateHabit;

public class UpdateHabitCommandValidator : AbstractValidator<UpdateHabitCommand>
{
    public UpdateHabitCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Name).NotEmpty().MaximumLength(100);
        RuleFor(c => c.TargetValue).GreaterThan(0).LessThanOrEqualTo(365);
        RuleFor(c => c.Unit).NotEmpty().MaximumLength(5);
    }
}
