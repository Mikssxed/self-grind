using MediatR;
using SelfGrind.Application.Tasks.Dtos;

namespace SelfGrind.Application.Tasks.Queries.GetDayActivity;

public class GetDayActivityQuery : IRequest<DayActivityDto>
{
    public DateOnly Date { get; set; }
}
