using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.User.Dtos;

namespace SelfGrind.Application.User.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler(
    ILogger<GetCurrentUserQueryHandler> logger,
    IUserContext userContext)
    : IRequestHandler<GetCurrentUserQuery, CurrentUserDto>
{
    public Task<CurrentUserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Handling GetCurrentUserQuery for user {userId}", currentUser.Id);

        return Task.FromResult(new CurrentUserDto
        {
            Username = currentUser.Username,
            Email = currentUser.Email,
        });
    }
}
