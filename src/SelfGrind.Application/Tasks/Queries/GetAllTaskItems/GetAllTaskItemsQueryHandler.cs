using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Tasks.Dtos;
using SelfGrind.Application.User;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Tasks.Queries.GetAllTaskItems;

public class GetAllTaskItemsQueryHandler(ILogger<GetAllTaskItemsQueryHandler> logger, IMapper mapper, ITasksRepository tasksRepository, IUserContext userContext) :
    IRequestHandler<GetAllTaskItemsQuery, TaskItemDto[]>
{
    public async Task<TaskItemDto[]> Handle(GetAllTaskItemsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetAllTaskItemsQuery");

        var userId = userContext.GetCurrentUser().Id;
        var taskItems = await tasksRepository.GetAllAsync(userId, cancellationToken);
        return mapper.Map<TaskItemDto[]>(taskItems);
    }
}