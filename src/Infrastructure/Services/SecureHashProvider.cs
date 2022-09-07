using SecretSauce = CCred.Sauce;
using System.Security.Cryptography;
using System.Text;

namespace Onion.Infrastructure.Services;

internal sealed class SecureHashProvider : ISecureHash
{
    public string HashPassword(string rawPassword, out string salt)
    {
        salt = SecretSauce.GenerateSalt(8);
        return SecretSauce.GetHash<SHA384>(rawPassword, salt, Encoding.UTF8);
    }

    public bool VerifyPassword(string rawPassword, string salt, string hashedPassword)
    {
        return SecretSauce.Verify<SHA384>(rawPassword, salt, hashedPassword, Encoding.UTF8);
    }
}
