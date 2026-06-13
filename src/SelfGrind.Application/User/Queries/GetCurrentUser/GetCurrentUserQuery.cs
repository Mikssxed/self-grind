using MediatR;
using SelfGrind.Application.User.Dtos;

namespace SelfGrind.Application.User.Queries.GetCurrentUser;

public class GetCurrentUserQuery : IRequest<CurrentUserDto>;
