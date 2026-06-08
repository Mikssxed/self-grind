using FluentValidation;

namespace SelfGrind.Application.Character.Commands.UnequipItem;

public class UnequipItemCommandValidator : AbstractValidator<UnequipItemCommand>
{
    public UnequipItemCommandValidator()
    {
        RuleFor(c => c.UserItemId).NotEmpty();
    }
}
