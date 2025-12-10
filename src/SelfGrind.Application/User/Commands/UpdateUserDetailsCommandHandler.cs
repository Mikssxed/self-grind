using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace SelfGrind.Application.User.Commands;

public class UpdateUserDetailsCommandHandler(ILogger<UpdateUserDetailsCommandHandler> logger, IUserContext userContext, IUserStore<Domain.Entities.User> userStore)
    : IRequestHandler<UpdateUserDetailsCommand>
{
    public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation("Updating user: {@UserId} with {@UserDetails}", user!.Id, request);

        var dbUser = await userStore.FindByIdAsync(user!.Id, cancellationToken);

        // if (dbUser == null) throw new NotFoundException(nameof(Domain.Entities.User), user!.Id); @todo uncomment when NotFoundException is available

        dbUser.Username = request.Username;

        await userStore.UpdateAsync(dbUser, cancellationToken);
    }
}