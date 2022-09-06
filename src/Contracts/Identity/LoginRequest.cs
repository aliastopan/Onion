namespace Onion.Contracts.Identity;

public record LoginRequest(
    string Username,
    string Password);
