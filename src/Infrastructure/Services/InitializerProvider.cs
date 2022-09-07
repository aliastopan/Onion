using Onion.Domain.Entities.Identity;
using Onion.Domain.Enums;

namespace Onion.Infrastructure.Services;

internal sealed class InitializerProvider : IInitializerService
{
    private readonly ISecureHashService _secureHashService;
    private readonly IDbContext _dbContext;

    public InitializerProvider(ISecureHashService secureHashService,
    IDbContext dbContext)
    {
        _secureHashService = secureHashService;
        _dbContext = dbContext;
    }

    public int Seed()
    {
        var user = new User
        {
            Id = Guid.Parse("9dd0aa01-3a6e-4159-8c7b-8ee4caa1d4ea"),
            Username = "einharan",
            Email = "einharan@gmail.com",
            HashedPassword = _secureHashService.HashPassword("LongPassword012", out var salt),
            Salt = salt,
            IsVerified = true,
            Role = UserRole.Developer,
        };

        _dbContext.Users.Add(user);
        return _dbContext.Commit();
    }
}
