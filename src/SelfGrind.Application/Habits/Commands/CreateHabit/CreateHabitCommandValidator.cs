using FluentValidation;

namespace SelfGrind.Application.Habits.Commands.CreateHabit;

public class CreateHabitCommandValidator: AbstractValidator<CreateHabitCommand>
{
    public CreateHabitCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(100);
        
        RuleFor(c => c.TargetValue)
            .GreaterThan(0).LessThanOrEqualTo(365);
        
        RuleFor(c => c.Unit).NotEmpty().MaximumLength(5);
    }
}