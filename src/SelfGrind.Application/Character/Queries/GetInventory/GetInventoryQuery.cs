using MediatR;
using SelfGrind.Application.Character.Dtos;

namespace SelfGrind.Application.Character.Queries.GetInventory;

public class GetInventoryQuery : IRequest<InventoryItemDto[]>;
