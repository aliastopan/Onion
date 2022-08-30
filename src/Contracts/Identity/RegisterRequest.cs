namespace Onion.Contracts.Identity;

public record RegisterRequest(
    string Username,
    string Email,
    string Password);
