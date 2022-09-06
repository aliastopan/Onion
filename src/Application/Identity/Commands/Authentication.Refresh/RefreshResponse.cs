namespace Onion.Application.Identity.Commands.Authentication.Refresh;

public record RefreshResponse(
    string Jwt,
    string RefreshToken);
