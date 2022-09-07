using Onion.Domain.Entities.Identity;
using Onion.Domain.Enums;

namespace Onion.Application.Identity.Commands.GrantRole;

public class GrantRoleCommandHandler
    : IRequestHandler<GrantRoleCommand, Result<GrantRoleResponse>>
{
    private readonly IDbContext _dbContext;
    private readonly ISecureHashService _secureHashService;

    public GrantRoleCommandHandler(
        IDbContext dbContext,
        ISecureHashService secureHashService)
    {
        _dbContext = dbContext;
        _secureHashService = secureHashService;
    }

    public Task<Result<GrantRoleResponse>> Handle(GrantRoleCommand request, CancellationToken cancellationToken)
    {
        var result = GrantRole(request);
        return Task.FromResult(result);
    }

    internal Result<GrantRoleResponse> GrantRole(GrantRoleCommand request)
    {
        var grantor = _dbContext.Users.Find(request.GrantorId);
        if(grantor is null)
        {
            return Result<GrantRoleResponse>.Unauthorized();
        }

        var validation = ValidatePermission(grantor, request.PermissionPassword);
        if(!validation.IsSuccess)
        {
            return Result<GrantRoleResponse>.Inherit(result: validation);
        }

        var grantee = _dbContext.Users.Find(request.GranteeId);
        if(grantee is null)
        {
            return Result<GrantRoleResponse>.Error();
        }

        var response = SetRole(grantee, request.Role);
        return Result<GrantRoleResponse>.Ok(response);
    }

    private GrantRoleResponse SetRole(User user, int role)
    {
        user.Role = (UserRole)role;
        _dbContext.Users.Update(user);
        _dbContext.Commit();

        return new GrantRoleResponse(user);
    }

    private Result ValidatePermission(User user, string password)
    {
        var isValid = _secureHashService.VerifyPassword(password, user.Salt, user.HashedPassword);
        if(!isValid)
        {
            return Result.Forbidden(Error.GrantRole.Denied);
        }

        return Result.Ok();
    }
}
