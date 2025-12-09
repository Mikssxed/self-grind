using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler(ILogger<CreateTaskCommandHandler> logger, IMapper mapper, ITasksRepository tasksRepository) : IRequestHandler<CreateTaskCommand, Guid>
{
    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new task {@Task}", request);
        var task = mapper.Map<TaskItem>(request);
        
        var id = await tasksRepository.Create(task);
        return id;
    }
}