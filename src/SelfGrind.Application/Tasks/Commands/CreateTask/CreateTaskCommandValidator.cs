using FluentValidation;
using SelfGrind.Domain.Constants;

namespace SelfGrind.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(c => c.RepetitionType)
            .Must(rt => Enum.IsDefined(typeof(TaskRepetitionType), rt))
            .WithMessage("Invalid repetition type.");

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        RuleFor(c => c.StartDate)
            .Must(date => date >= today)
            .WithMessage("Start date cannot be in the past.")
            .LessThanOrEqualTo(c => c.EndDate)
            .WithMessage("Start date must be earlier than or equal to end date.");

        RuleFor(c => c.EndDate)
            .Must(date => date >= today)
            .WithMessage("End date cannot be in the past.")
            .GreaterThanOrEqualTo(c => c.StartDate)
            .WithMessage("End date must be later than or equal to start date.");

        RuleFor(c => c.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(c => c.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(c => c.Attribute)
            .NotNull();

        RuleFor(c => c.DaysOfWeek)
            .Must((c, days) =>
            {
                if (c.RepetitionType == TaskRepetitionType.Weekly)
                    return days != null && days.All(d => d >= 0 && d <= (DayOfWeek)6);

                return days == null || days == new List<DayOfWeek>();
            })
            .WithMessage(c =>
                c.RepetitionType == TaskRepetitionType.Weekly
                    ? "Days of week must be between 0 (Sunday) and 6 (Saturday)."
                    : "Days of week should be null for non-weekly repetition types."
            );

        RuleFor(c => c.RepeatInterval)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(7);
    }
}