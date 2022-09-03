using Onion.Domain.Enums;

namespace Onion.Domain.Entities.Identity;

public class User
{
    public User(string username, string email, string hashedPassword, string salt)
    {
        Id = Guid.NewGuid();
        Username = username;
        Email = email;
        Role = UserRole.Standard;
        IsVerified = false;
        HashedPassword = hashedPassword;
        Salt = salt;
        CreationDate = DateTimeOffset.Now;
        LastLoggedIn = DateTimeOffset.Now;
    }

    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public UserRole Role { get; set; }
    public bool IsVerified { get; set; }
    public string HashedPassword { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset LastLoggedIn { get; set; }
}
