using Mapster;
using Onion.Application.Identity.Commands.Authentication;
using Onion.Application.Identity.Commands.Registration;
using Onion.Contracts.Identity;

namespace Onion.Api.Mapping;

public static class IdentityMapExtensions
{
    public static RegisterCommand AsCommand(this RegisterRequest request)
    {
        return request.Adapt<RegisterCommand>();
    }

    public static LoginCommand AsCommand(this LoginRequest request)
    {
        return request.Adapt<LoginCommand>();
    }
}
