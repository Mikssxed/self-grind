using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Character.Dtos;
using SelfGrind.Application.User;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Character.Queries.GetInventory;

public class GetInventoryQueryHandler(
    ILogger<GetInventoryQueryHandler> logger,
    IUserItemsRepository userItemsRepository,
    IUserContext userContext) : IRequestHandler<GetInventoryQuery, InventoryItemDto[]>
{
    public async Task<InventoryItemDto[]> Handle(GetInventoryQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Handling GetInventoryQuery for user {userId}", currentUser.Id);

        var userItems = await userItemsRepository.GetForUserAsync(currentUser.Id, cancellationToken);

        return userItems
            .OrderBy(ui => (int)ui.Item.Rarity)
            .ThenBy(ui => ui.Item.Name)
            .Select(ui => new InventoryItemDto
            {
                UserItemId = ui.Id,
                Name = ui.Item.Name,
                Bonus = ui.Item.Bonus,
                Emoji = ui.Item.Emoji,
                Rarity = ui.Item.Rarity,
                IsEquipped = ui.IsEquipped,
            })
            .ToArray();
    }
}
