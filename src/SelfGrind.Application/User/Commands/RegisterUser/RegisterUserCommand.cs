using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SelfGrind.Application.User.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<Results<Ok, ValidationProblem>>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Username { get; set; }
}