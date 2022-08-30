namespace Onion.Domain.Entities.Identity;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public bool IsVerified { get; set; }
    public string HashedPassword { get; set; } = null!;
    public string Salt { get; set; } = null!;
}
