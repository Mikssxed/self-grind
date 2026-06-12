using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Common;
using SelfGrind.Application.Tasks.Dtos;
using SelfGrind.Application.User;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Tasks.Queries.GetAllTaskItems;

public class GetAllTaskItemsQueryHandler(ILogger<GetAllTaskItemsQueryHandler> logger, IMapper mapper, ITasksRepository tasksRepository, IUserContext userContext) :
    IRequestHandler<GetAllTaskItemsQuery, PagedResult<TaskItemDto>>
{
    public async Task<PagedResult<TaskItemDto>> Handle(GetAllTaskItemsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetAllTaskItemsQuery");

        var userId = userContext.GetCurrentUser().Id;
        var (taskItems, totalCount) = await tasksRepository.GetAllAsync(userId, request.Page, request.PageSize, cancellationToken);

        return new PagedResult<TaskItemDto>
        {
            Items = mapper.Map<TaskItemDto[]>(taskItems),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
        };
    }
}
