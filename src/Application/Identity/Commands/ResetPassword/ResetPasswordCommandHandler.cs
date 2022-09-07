namespace Onion.Application.Identity.Commands.ResetPassword;

public class ResetPasswordCommandHandler
    :IRequestHandler<ResetPasswordCommand, Result<ResetPasswordResponse>>
{
    private readonly IDbContext _dbContext;
    private readonly ISecureHashService _secureHashService;

    public ResetPasswordCommandHandler(
        IDbContext dbContext,
        ISecureHashService secureHashService)
    {
        _dbContext = dbContext;
        _secureHashService = secureHashService;
    }

    public Task<Result<ResetPasswordResponse>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = ResetPassword(request);
        return Task.FromResult(result);
    }

    internal Result<ResetPasswordResponse> ResetPassword(ResetPasswordCommand request)
    {
        var isValid = request.TryValidate(out var errors);
        if(!isValid)
        {
            return Result<ResetPasswordResponse>.Invalid(errors);
        }

        var user = _dbContext.Users.Find(request.UserId);
        if(user is null)
        {
            return Result<ResetPasswordResponse>.Unauthorized();
        }

        var validation = ValidatePassword(request.NewPassword, request.OldPassword, user.Salt, user.HashedPassword);
        if(!validation.IsSuccess)
        {
            return Result<ResetPasswordResponse>.Inherit(result: validation);
        }

        var hash = _secureHashService.HashPassword(request.NewPassword, out var salt);
        user.HashedPassword = hash;
        user.Salt = salt;
        _dbContext.Users.Update(user);
        _dbContext.Commit();

        var response = new ResetPasswordResponse(user);
        return Result<ResetPasswordResponse>.Ok(response);
    }

    private Result ValidatePassword(string newPassword, string oldPassword, string salt, string hashedPassword)
    {
        var isValid = _secureHashService.VerifyPassword(oldPassword, salt, hashedPassword);
        if(!isValid)
        {
            return Result.Unauthorized(Error.ResetPassword.IncorrectPassword);
        }

        var isNew = !_secureHashService.VerifyPassword(newPassword, salt, hashedPassword);
        if(!isNew)
        {
            return Result.Error(Error.ResetPassword.SameOldPassword);
        }

        return Result.Ok();
    }
}
