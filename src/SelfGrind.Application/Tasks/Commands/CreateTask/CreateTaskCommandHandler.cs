using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.User;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler(ILogger<CreateTaskCommandHandler> logger, IMapper mapper, ITasksRepository tasksRepository, IUserContext userContext) : IRequestHandler<CreateTaskCommand, Guid>
{
    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Creating a new task {@Task} for user {@User}", request, currentUser);
        var task = mapper.Map<TaskItem>(request);
        task.UserId = currentUser.Id;
        
        var id = await tasksRepository.Create(task);
        return id;
    }
}