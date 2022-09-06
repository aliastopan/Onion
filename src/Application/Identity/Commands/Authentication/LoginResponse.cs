using Onion.Domain.Entities.Identity;

namespace Onion.Application.Identity.Commands.Authentication;

public record LoginResponse(User User, string Jwt);
