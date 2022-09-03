using Onion.Domain.Entities.Identity;

namespace Onion.Application.Identity.Commands.Registration;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, Result<RegisterCommandResponse>>
{
    private readonly IDbContext _dbContext;
    private readonly ISecureHash _secureHash;

    public RegisterCommandHandler(
        IDbContext dbContext,
        ISecureHash secureHash)
    {
        _dbContext = dbContext;
        _secureHash = secureHash;
    }

    public Task<Result<RegisterCommandResponse>> Handle(RegisterCommand request,
        CancellationToken cancellationToken)
    {
        Result<RegisterCommandResponse> result;
        var isValid = request.TryValidate(out var errors);
        if(!isValid)
        {
            result = Result<RegisterCommandResponse>.Invalid(errors);
            return Task.FromResult(result);
        }

        var validation = ValidateEntry(request.Username, request.Email);
        if(!validation.IsSuccess)
        {
            errors = validation.Errors.ToArray();
            result = Result<RegisterCommandResponse>.Conflict(errors);
            return Task.FromResult(result);
        }

        var user = CreateUser(request.Username, request.Email, request.Password);
        var response = new RegisterCommandResponse(user.Id, user.Username);
        result = Result<RegisterCommandResponse>.Ok(response);
        return Task.FromResult(result);
    }

    private Result ValidateEntry(string username, string email)
    {
        User user = _dbContext.Users.Search(username)!;
        if(user is not null)
        {
            return Result.Conflict(Error.Registration.UsernameTaken);
        }

        user = _dbContext.Users.SearchByEmail(email)!;
        if(user is not null)
        {
            return Result.Conflict(Error.Registration.EmailInUse);
        }

        return Result.Ok();
    }

    private User CreateUser(string username, string email, string password)
    {
        var hash = _secureHash.HashPassword(password, out string salt);
        var user = new User(username, email, hash, salt);
        _dbContext.Users.Add(user);
        _dbContext.Commit();
        return user;
    }
}
