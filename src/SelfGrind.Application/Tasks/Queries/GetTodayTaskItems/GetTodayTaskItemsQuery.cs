using MediatR;
using SelfGrind.Application.Tasks.Dtos;

namespace SelfGrind.Application.Tasks.Queries.GetTodayTaskItems;

public class GetTodayTaskItemsQuery : IRequest<TodayTaskItemDto[]>
{
}
