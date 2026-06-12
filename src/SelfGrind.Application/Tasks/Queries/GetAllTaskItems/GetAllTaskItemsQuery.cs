using MediatR;
using SelfGrind.Application.Common;
using SelfGrind.Application.Tasks.Dtos;

namespace SelfGrind.Application.Tasks.Queries.GetAllTaskItems;

public class GetAllTaskItemsQuery : IRequest<PagedResult<TaskItemDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
