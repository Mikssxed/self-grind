using MediatR;

namespace SelfGrind.Application.Character.Commands.EquipItem;

public class EquipItemCommand : IRequest
{
    public Guid UserItemId { get; set; }
}
