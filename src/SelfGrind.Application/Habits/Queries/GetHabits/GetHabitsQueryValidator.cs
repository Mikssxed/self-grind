using FluentValidation;

namespace SelfGrind.Application.Habits.Queries.GetHabits;

public class GetHabitsQueryValidator : AbstractValidator<GetHabitsQuery>
{
    public GetHabitsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}
