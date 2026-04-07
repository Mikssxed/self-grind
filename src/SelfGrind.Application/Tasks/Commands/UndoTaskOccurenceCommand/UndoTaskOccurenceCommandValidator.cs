using FluentValidation;
using SelfGrind.Application.Tasks.Commands.CompleteTaskOccurence;

namespace SelfGrind.Application.Tasks.Commands.UndoTaskOccurenceCommand;

public class UndoTaskOccurenceCommandValidator : AbstractValidator<UndoTaskOccurenceCommand>
{
    public UndoTaskOccurenceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
