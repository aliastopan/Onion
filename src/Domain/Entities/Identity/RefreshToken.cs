#nullable disable

namespace Onion.Domain.Entities.Identity;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public string JwtId { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsUsed { get; set; }
    public bool IsInvalidated { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set;}
}
