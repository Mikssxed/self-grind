using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Behaviors;
using SelfGrind.Application.Character.Services;
using SelfGrind.Domain.Interfaces;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.User.Commands.RegisterUser;

public class RegisterUserCommandHandler(
    ILogger<RegisterUserCommandHandler> logger,
    UserManager<Domain.Entities.User> userManager,
    IEmailService emailService,
    IUsersRepository usersRepository,
    IUserItemGranter userItemGranter,
    IConfiguration configuration)
    : IRequestHandler<RegisterUserCommand>
{
    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Registering user {username}", request.Username);

        var user = new Domain.Entities.User
        {
            UserName = request.Username,
            Email = request.Email
        };

        var result = await userManager.CreateAsync(user, request.Password);
        result.ThrowIfFailed();

        await usersRepository.SeedStatsAsync(user.Id, cancellationToken);

        await userItemGranter.GrantUnlockedItemsAsync(user.Id, user.Level, cancellationToken);

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var frontendUrl = configuration["AppSettings:FrontendUrl"] ?? "http://localhost:5173";
        var confirmationLink = $"{frontendUrl}/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";

        // A failed confirmation email must not fail registration: the account is already created and
        // login does not require a confirmed email. Log and continue so a transient SMTP outage is non-fatal.
        try
        {
            await emailService.SendConfirmationEmailAsync(user.Email!, user.UserName!, confirmationLink);
            logger.LogInformation("User {username} registered successfully and confirmation email sent", request.Username);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "User {username} registered but the confirmation email could not be sent", request.Username);
        }
    }
}