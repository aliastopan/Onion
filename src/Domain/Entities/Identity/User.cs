using Onion.Domain.Enums;

namespace Onion.Domain.Entities.Identity;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public UserRole Role { get; set; }
    public bool IsVerified { get; set; }
    public string HashedPassword { get; set; } = null!;
    public string Salt { get; set; } = null!;
}
