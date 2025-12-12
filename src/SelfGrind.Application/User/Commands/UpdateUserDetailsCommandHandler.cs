using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SelfGrind.Domain.Exceptions;

namespace SelfGrind.Application.User.Commands;

public class UpdateUserDetailsCommandHandler(ILogger<UpdateUserDetailsCommandHandler> logger, IUserContext userContext, IUserStore<Domain.Entities.User> userStore)
    : IRequestHandler<UpdateUserDetailsCommand>
{
    public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation("Updating user: {@UserId} with {@UserDetails}", user!.Id, request);

        var dbUser = await userStore.FindByIdAsync(user!.Id, cancellationToken);

        if (dbUser == null) throw new NotFoundException(nameof(Domain.Entities.User), user!.Id);

        dbUser.UserName = request.Username;

        await userStore.UpdateAsync(dbUser, cancellationToken);
    }
}