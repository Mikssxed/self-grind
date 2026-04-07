using MediatR;

namespace SelfGrind.Application.Tasks.Commands.UndoTaskOccurenceCommand;

public class UndoTaskOccurenceCommand(Guid id) : IRequest<Guid>
{
    public Guid Id { get; set; } = id;
}