using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Tasks.Dtos;
using SelfGrind.Application.User;
using SelfGrind.Domain.Exceptions;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Tasks.Queries.GetTaskItemById;

public class GetTaskItemByIdQueryHandler(ILogger<GetTaskItemByIdQueryHandler> logger, IMapper mapper, ITasksRepository tasksRepository, IUserContext userContext) :
    IRequestHandler<GetTaskItemByIdQuery, TaskItemDto>
{
    public async Task<TaskItemDto> Handle(GetTaskItemByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetTaskItemByIdQuery for TaskItemId: {taskItemId}", request.Id);
        
        var userId = userContext.GetCurrentUser().Id;
        var task = await tasksRepository.GetByIdAsync(userId, request.Id, cancellationToken);

        if (task == null)
        {
            throw new NotFoundException("task", request.Id.ToString());
        }

        return mapper.Map<TaskItemDto>(task);
    }
}