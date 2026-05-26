using FluentValidation;

namespace SelfGrind.Application.Tasks.Queries.GetContributionGrid;

public class GetContributionGridQueryValidator : AbstractValidator<GetContributionGridQuery>
{
    public GetContributionGridQueryValidator()
    {
        RuleFor(x => x.Year)
            .InclusiveBetween(2020, 2100);
    }
}
