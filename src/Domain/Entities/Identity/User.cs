using Onion.Domain.Enums;

namespace Onion.Domain.Entities.Identity;

public class User
{
    public User()
    {
        Role = UserRole.Standard;
        CreationDate = DateTime.UtcNow;
        LastLoggedIn = DateTime.UtcNow;
    }

    public User(string username, string email, string hashedPassword, string salt, DateTime creationDate)
    {
        Id = Guid.NewGuid();
        Username = username;
        Email = email;
        Role = UserRole.Standard;
        IsVerified = false;
        HashedPassword = hashedPassword;
        Salt = salt;
        CreationDate = creationDate;
        LastLoggedIn = creationDate;
    }

    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public UserRole Role { get; set; }
    public bool IsVerified { get; set; }
    public string HashedPassword { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public DateTime CreationDate { get; set; }
    public DateTime LastLoggedIn { get; set; }
}
