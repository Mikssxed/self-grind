using FluentValidation;

namespace SelfGrind.Application.Tasks.Commands.CompleteTaskOccurence;

public class CompleteTaskOccurenceCommandValidator : AbstractValidator<CompleteTaskOccurenceCommand>
{
    public CompleteTaskOccurenceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
