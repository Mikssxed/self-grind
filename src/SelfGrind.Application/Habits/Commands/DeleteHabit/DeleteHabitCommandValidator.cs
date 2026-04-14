using FluentValidation;

namespace SelfGrind.Application.Habits.Commands.DeleteHabit;

public class DeleteHabitCommandValidator : AbstractValidator<DeleteHabitCommand>
{
    public DeleteHabitCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
