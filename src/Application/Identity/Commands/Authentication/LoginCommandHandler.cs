namespace Onion.Application.Identity.Commands.Authentication;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IDbContext _dbContext;
    private readonly ISecureHash _secureHash;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(
        IDbContext dbContext,
        ISecureHash secureHash,
        IJwtService jwtService)
    {
        _dbContext = dbContext;
        _secureHash = secureHash;
        _jwtService = jwtService;
    }

    public Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = Login(request);
        return Task.FromResult(result);
    }

    internal Result<LoginResponse> Login(LoginCommand request)
    {
        var isValid = request.TryValidate(out var errors);
        if(!isValid)
        {
            return Result<LoginResponse>.Invalid(errors);
        }

        var user = _dbContext.Users.Search(request.Username);
        if(user is null)
        {
            return Result<LoginResponse>.Unauthorized(Error.Authentication.UserNotRegistered);
        }

        var validation = ValidatePassword(request.Password, user.Salt, user.HashedPassword);
        if(!validation.IsSuccess)
        {
            return Result<LoginResponse>.Inherit(result: validation);
        }

        user.LastLoggedIn = DateTimeOffset.Now;
        _dbContext.Users.Update(user);
        _dbContext.Commit();

        var jwt = _jwtService.GenerateJwt(user);
        var response = new LoginResponse(user, jwt);
        return Result<LoginResponse>.Ok(response);
    }

    private Result ValidatePassword(string password, string salt, string hashedPassword)
    {
        var isValid = _secureHash.VerifyPassword(password, salt, hashedPassword);
        if(!isValid)
        {
            return Result.Unauthorized(Error.Authentication.IncorrectPassword);
        }

        return Result.Ok();
    }
}
