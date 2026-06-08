using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.User;
using SelfGrind.Domain.Exceptions;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Character.Commands.UnequipItem;

public class UnequipItemCommandHandler(
    ILogger<UnequipItemCommandHandler> logger,
    IUserItemsRepository userItemsRepository,
    IUserContext userContext) : IRequestHandler<UnequipItemCommand>
{
    public async Task Handle(UnequipItemCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Handling UnequipItemCommand for user {userId} on userItem {userItemId}", currentUser.Id, request.UserItemId);

        var userItem = await userItemsRepository.GetForUserAsync(currentUser.Id, request.UserItemId, cancellationToken);
        if (userItem is null) throw new NotFoundException("user item", request.UserItemId.ToString());

        if (!userItem.IsEquipped) return;

        userItem.IsEquipped = false;
        await userItemsRepository.SaveChangesAsync(cancellationToken);
    }
}
