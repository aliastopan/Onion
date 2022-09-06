using Onion.Domain.Entities.Identity;

namespace Onion.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateJwt(User user);
    RefreshToken GenerateRefreshToken(string jwt, User user);
    Result Refresh(ref string jwt, ref string refreshToken);
}
