namespace SelfGrind.Domain.Exceptions;

public class AuthenticationException(string message): Exception($"Something went wrong: {message}.");