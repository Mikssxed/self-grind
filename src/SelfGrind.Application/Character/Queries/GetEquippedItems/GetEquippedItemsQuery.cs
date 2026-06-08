using MediatR;
using SelfGrind.Application.Character.Dtos;

namespace SelfGrind.Application.Character.Queries.GetEquippedItems;

public class GetEquippedItemsQuery : IRequest<EquippedItemDto[]>;
