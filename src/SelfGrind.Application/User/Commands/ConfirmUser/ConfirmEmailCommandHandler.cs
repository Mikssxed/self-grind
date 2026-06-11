using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Behaviors;

namespace SelfGrind.Application.User.Commands.ConfirmUser;

public class ConfirmEmailCommandHandler(
    ILogger<ConfirmEmailCommandHandler> logger,
    UserManager<Domain.Entities.User> userManager)
    : IRequestHandler<ConfirmEmailCommand>
{
    public async Task Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Confirming email for user: {UserId}", request.UserId);

        var user = await userManager.FindByIdAsync(request.UserId);

        if (user == null)
        {
            // Fail the same way as an invalid token so responses don't reveal whether the user id exists
            IdentityResult.Failed(new IdentityError { Code = "InvalidToken", Description = "Invalid token." }).ThrowIfFailed();
            return;
        }

        var code = request.ConfirmationCode;
        
        var result = await userManager.ConfirmEmailAsync(user, code);
        
        result.ThrowIfFailed();
    }
}