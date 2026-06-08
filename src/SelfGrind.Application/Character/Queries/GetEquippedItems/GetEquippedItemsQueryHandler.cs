using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Character.Dtos;
using SelfGrind.Application.User;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Character.Queries.GetEquippedItems;

public class GetEquippedItemsQueryHandler(
    ILogger<GetEquippedItemsQueryHandler> logger,
    IUserItemsRepository userItemsRepository,
    IUserContext userContext) : IRequestHandler<GetEquippedItemsQuery, EquippedItemDto[]>
{
    public async Task<EquippedItemDto[]> Handle(GetEquippedItemsQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Handling GetEquippedItemsQuery for user {userId}", currentUser.Id);

        var userItems = await userItemsRepository.GetForUserAsync(currentUser.Id, cancellationToken);

        return userItems
            .Where(ui => ui.IsEquipped)
            .OrderBy(ui => ui.Item.Name)
            .Select(ui => new EquippedItemDto
            {
                UserItemId = ui.Id,
                Name = ui.Item.Name,
                Type = ui.Item.Type,
                Bonus = ui.Item.Bonus,
                Emoji = ui.Item.Emoji,
                Variant = ui.Item.Variant,
            })
            .ToArray();
    }
}
