using Onion.Domain.Entities.Identity;

namespace Onion.Application.Identity.Commands.Registration;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    private readonly IDbContext _dbContext;
    private readonly ISecureHashService _secureHashService;
    private readonly IDateTimeService _dateTimeService;

    public RegisterCommandHandler(
        IDbContext dbContext,
        ISecureHashService secureHashService,
        IDateTimeService dateTimeService)
    {
        _dbContext = dbContext;
        _secureHashService = secureHashService;
        _dateTimeService = dateTimeService;
    }

    public Task<Result<RegisterResponse>> Handle(RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var result = Register(request);
        return Task.FromResult(result);
    }

    internal Result<RegisterResponse> Register(RegisterCommand request)
    {
        var isValid = request.TryValidate(out var errors);
        if(!isValid)
        {
            return Result<RegisterResponse>.Invalid(errors);
        }

        var validation = ValidateEntry(request.Username, request.Email);
        if(!validation.IsSuccess)
        {
            return Result<RegisterResponse>.Inherit(result: validation);
        }

        var response = CreateUser(request.Username, request.Email, request.Password);
        return Result<RegisterResponse>.Ok(response);
    }

    private RegisterResponse CreateUser(string username, string email, string password)
    {
        var hash = _secureHashService.HashPassword(password, out string salt);
        var user = new User(username, email, hash, salt, _dateTimeService.UtcNow);
        _dbContext.Users.Add(user);
        _dbContext.Commit();

        return new RegisterResponse(user);
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
}
