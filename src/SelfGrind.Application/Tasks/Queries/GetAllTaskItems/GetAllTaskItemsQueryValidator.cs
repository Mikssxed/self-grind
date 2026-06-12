using FluentValidation;

namespace SelfGrind.Application.Tasks.Queries.GetAllTaskItems;

public class GetAllTaskItemsQueryValidator : AbstractValidator<GetAllTaskItemsQuery>
{
    public GetAllTaskItemsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}
