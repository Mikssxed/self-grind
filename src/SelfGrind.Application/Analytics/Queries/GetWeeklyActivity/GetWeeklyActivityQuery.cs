using MediatR;
using SelfGrind.Application.Analytics.Dtos;

namespace SelfGrind.Application.Analytics.Queries.GetWeeklyActivity;

public class GetWeeklyActivityQuery : IRequest<WeeklyActivityDto>
{
    public DateOnly? WeekStart { get; set; }
}
