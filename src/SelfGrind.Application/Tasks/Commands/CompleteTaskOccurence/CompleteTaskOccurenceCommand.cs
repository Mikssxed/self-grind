using MediatR;

namespace SelfGrind.Application.Tasks.Commands.CompleteTaskOccurence;

public class CompleteTaskOccurenceCommand(Guid id) : IRequest<Guid>
{
    public Guid Id { get; set; } = id;
}