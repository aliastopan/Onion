namespace Onion.Application.Common.Interfaces;

public interface ISecureHashService
{
    string HashPassword(string rawPassword, out string salt);
    bool VerifyPassword(string rawPassword, string salt, string hashedPassword);
}
