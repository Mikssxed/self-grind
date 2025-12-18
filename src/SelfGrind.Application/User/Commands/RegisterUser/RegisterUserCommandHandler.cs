using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Behaviors;
using SelfGrind.Domain.Interfaces;

namespace SelfGrind.Application.User.Commands.RegisterUser;

public class RegisterUserCommandHandler(
    ILogger<RegisterUserCommandHandler> logger,
    UserManager<Domain.Entities.User> userManager,
    IEmailService emailService,
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

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var frontendUrl = configuration["AppSettings:FrontendUrl"] ?? "http://localhost:5173";
        var confirmationLink = $"{frontendUrl}/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";

        await emailService.SendConfirmationEmailAsync(user.Email!, user.UserName!, confirmationLink);

        logger.LogInformation("User {username} registered successfully and confirmation email sent", request.Username);
    }
}