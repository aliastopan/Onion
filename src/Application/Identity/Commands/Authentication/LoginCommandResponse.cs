using Onion.Domain.Entities.Identity;

namespace Onion.Application.Identity.Commands.Authentication;

public record LoginCommandResponse(User User);
