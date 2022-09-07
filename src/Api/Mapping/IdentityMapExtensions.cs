using Mapster;
using Onion.Application.Identity.Commands.Authentication;
using Onion.Application.Identity.Commands.Registration;
using Onion.Application.Identity.Commands.ResetPassword;
using Onion.Application.Identity.Commands.GrantRole;
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

    public static ResetPasswordCommand AsCommand(this ResetPasswordRequest request)
    {
        return request.Adapt<ResetPasswordCommand>();
    }

    public static GrantRoleCommand AsCommand(this GrantRoleRequest request)
    {
        return request.Adapt<GrantRoleCommand>();
    }
}
