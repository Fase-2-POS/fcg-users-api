namespace Fcg.Contracts;

public record UserCreatedEvent(
    int UserId,
    string Nome,
    string Email);
