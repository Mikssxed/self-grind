using FluentValidation;

namespace SelfGrind.Application.Analytics.Queries.GetXpPerWeek;

public class GetXpPerWeekQueryValidator : AbstractValidator<GetXpPerWeekQuery>
{
    public GetXpPerWeekQueryValidator()
    {
        RuleFor(x => x.Weeks).InclusiveBetween(1, 26);
    }
}
