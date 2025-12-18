namespace SelfGrind.Domain.Exceptions;

public class BadRequestException(Dictionary<string, string[]> errors) : Exception("One or more validation errors occurred.")
{
    public Dictionary<string, string[]> Errors { get; } = errors;
}
