using FluentValidation;
using SelfGrind.Domain.Constants;

namespace SelfGrind.Application.Tasks.Commands.LogActivity;

public class LogActivityCommandValidator : AbstractValidator<LogActivityCommand>
{
    public LogActivityCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(c => c.Notes)
            .MaximumLength(500);

        RuleFor(c => c.Exp)
            .GreaterThan(0)
            .LessThanOrEqualTo(500);

        RuleFor(c => c.Attribute)
            .Must(a => Enum.IsDefined(typeof(BaseAttribute), a))
            .WithMessage("Invalid attribute.");
    }
}
