using MediatR;

namespace SelfGrind.Application.Character.Commands.UnequipItem;

public class UnequipItemCommand : IRequest
{
    public Guid UserItemId { get; set; }
}
