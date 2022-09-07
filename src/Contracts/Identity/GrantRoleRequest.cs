namespace Onion.Contracts.Identity;

public record GrantRoleRequest(
    Guid GrantorId,
    string PermissionPassword,
    Guid GranteeId,
    int Role);
