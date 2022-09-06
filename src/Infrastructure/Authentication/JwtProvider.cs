using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AnnotatedResult;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Onion.Domain.Entities.Identity;

namespace Onion.Infrastructure.Authentication;

internal sealed class JwtProvider : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly JwtValidator _jwtValidator;
    private readonly IDbContext _dbContext;

    public JwtProvider(
        IOptions<JwtSettings> jwtSettings,
        JwtValidator jwtValidator,
        IDbContext dbContext)
    {
        _jwtSettings = jwtSettings.Value;
        _jwtValidator = jwtValidator;
        _dbContext = dbContext;
    }

    public string GenerateJwt(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha384);
        var claims = new[]
        {
            new Claim(JwtClaimTypes.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtClaimTypes.Sub, user.Id.ToString()),
            new Claim(JwtClaimTypes.UniqueName, user.Username),
            new Claim(JwtClaimTypes.Role, user.Role.ToString()),
        };

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwt = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime),
            claims: claims,
            signingCredentials: signingCredentials);

        return jwtHandler.WriteToken(jwt);
    }

    public string GenerateJwt(ClaimsPrincipal principal, out User user)
    {
        var sub = principal.Claims.Single(x => x.Type == JwtClaimTypes.Sub).Value;
        var userId = Guid.Parse(sub);
        user = _dbContext.Users.Find(userId)!;
        return GenerateJwt(user);
    }

    public RefreshToken GenerateRefreshToken(string jwt, User user)
    {
        var principal = GetPrincipalFromToken(jwt);
        var jti = principal.Claims.Single(x => x.Type == JwtClaimTypes.Jti).Value;
        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            JwtId = jti,
            CreationDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.Add(_jwtSettings.RefreshLifeTime),
            User = user
        };
        _dbContext.RefreshTokens.Add(refreshToken);
        _dbContext.Commit();
        return refreshToken;
    }

    public Result<(string jwt, string refreshToken)> Refresh(string jwt, string refreshToken)
    {
        var principal = GetPrincipalFromToken(jwt);
        if(principal is null)
        {
            return Result<(string jwt, string refreshToken)>.Unauthorized();
        }

        var validation = ValidateToken(principal, refreshToken);
        if(!validation.IsSuccess)
        {
            return Result<(string jwt, string refreshToken)>.Inherit(result: validation);
        }

        var refreshedToken = GenerateToken(principal);
        return Result<(string jwt, string refreshToken)>.Ok(refreshedToken);
    }

    internal (string jwt, string refreshToken) GenerateToken(ClaimsPrincipal principal)
    {
        var jwt = GenerateJwt(principal, out var user);
        var refreshToken = GenerateRefreshToken(jwt, user).Token;
        return (jwt, refreshToken);
    }

    private Result ValidateToken(ClaimsPrincipal principal, string refreshToken)
    {
        var expiry = principal.Claims.Single(x => x.Type == JwtClaimTypes.Exp).Value;
        var expiryDateUnix = long.Parse(expiry);
        var expiryDateUtc = DateTime.UnixEpoch.AddSeconds(expiryDateUnix);
        if(expiryDateUtc > DateTime.UtcNow)
        {
            return Result.Unauthorized();
        }

        var token = _dbContext.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);
        if(token is null
            || token.ExpiryDate > DateTime.UtcNow
            || token.IsInvalidated
            || token.IsUsed)
        {
            return Result.Unauthorized();
        }

        var jti = principal.Claims.Single(x => x.Type == JwtClaimTypes.Jti).Value;
        if(token.JwtId != jti)
        {
            return Result.Unauthorized();
        }

        MarkRefreshTokenAsUsed(token);
        return Result.Ok();
    }

    private ClaimsPrincipal GetPrincipalFromToken(string jwt)
    {
        try
        {
            var validationParameters = _jwtValidator.RefreshValidationParameters();
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(jwt, validationParameters, out var securityToken);
            if(!HasValidSecurityAlgorithm(securityToken))
            {
                return null!;
            }

            return principal;
        }
        catch
        {
            return null!;
        }
    }

    private static bool HasValidSecurityAlgorithm(SecurityToken securityToken)
    {
        var securityAlgorithm = SecurityAlgorithms.HmacSha384;
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        return jwtSecurityToken is not null && jwtSecurityToken!.Header.Alg
            .Equals(securityAlgorithm, StringComparison.InvariantCultureIgnoreCase);
    }

    private void MarkRefreshTokenAsUsed(RefreshToken refreshToken)
    {
        refreshToken.IsUsed = true;
        _dbContext.RefreshTokens.Update(refreshToken);
        _dbContext.Commit();
    }
}
