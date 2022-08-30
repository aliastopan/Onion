namespace Onion.Application.Identity.Commands.Registration;

public record RegisterCommandResponse(
    Guid UserId,
    string Username);
