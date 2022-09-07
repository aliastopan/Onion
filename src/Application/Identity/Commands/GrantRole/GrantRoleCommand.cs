namespace Onion.Application.Identity.Commands.GrantRole;

public record GrantRoleCommand(
    Guid GrantorId,
    string PermissionPassword,
    Guid GranteeId,
    int Role) : IRequest<Result<GrantRoleResponse>>;
