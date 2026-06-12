using MediatR;
using SelfGrind.Application.Common;
using SelfGrind.Application.Tasks.Dtos;

namespace SelfGrind.Application.Habits.Queries.GetHabits;

public class GetHabitsQuery : IRequest<PagedResult<HabitDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
