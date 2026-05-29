using FluentValidation;

namespace SelfGrind.Application.Analytics.Queries.GetStatGrowth;

public class GetStatGrowthQueryValidator : AbstractValidator<GetStatGrowthQuery>
{
    public GetStatGrowthQueryValidator()
    {
        RuleFor(x => x.Weeks).InclusiveBetween(1, 26);
    }
}
