using FluentValidation;

namespace SelfGrind.Application.User.Commands;

public class UpdateUserDetailsCommandValidator : AbstractValidator<UpdateUserDetailsCommand>
{
    public UpdateUserDetailsCommandValidator()
    {
        RuleFor()
    }
}