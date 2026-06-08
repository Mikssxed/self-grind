using FluentValidation;

namespace SelfGrind.Application.Character.Commands.EquipItem;

public class EquipItemCommandValidator : AbstractValidator<EquipItemCommand>
{
    public EquipItemCommandValidator()
    {
        RuleFor(c => c.UserItemId).NotEmpty();
    }
}
