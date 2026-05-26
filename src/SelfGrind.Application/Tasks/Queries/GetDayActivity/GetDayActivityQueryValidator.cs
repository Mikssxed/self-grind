using FluentValidation;

namespace SelfGrind.Application.Tasks.Queries.GetDayActivity;

public class GetDayActivityQueryValidator : AbstractValidator<GetDayActivityQuery>
{
    public GetDayActivityQueryValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty();
    }
}
