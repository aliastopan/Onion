namespace Onion.Application.Identity.Commands.Authentication.Refresh;

public record RefreshCommand(
    string Jwt,
    string RefreshToken) : IRequest<Result<RefreshResponse>>;