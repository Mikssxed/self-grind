using MediatR;

namespace SelfGrind.Application.Tasks.Commands.DeleteTask;

public class DeleteTaskCommand(Guid id) : IRequest
{
    public Guid Id { get; } = id;
}
