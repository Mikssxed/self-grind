using FluentValidation;

namespace SelfGrind.Application.User.Commands.ConfirmUser;

public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.ConfirmationCode)
            .NotEmpty().WithMessage("Token is required.");
    }
}