using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace Onion.Infrastructure.Authentication;

internal sealed class JwtValidator
{
    private readonly IConfiguration _configuration;

    public JwtValidator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static TokenValidationParameters JwtValidationParameters(IConfiguration configuration)
    {
        var secret = configuration[JwtSettings.Element.Secret];
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = configuration[JwtSettings.Element.Issuer],
            ValidAudience = configuration[JwtSettings.Element.Audience],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ClockSkew = TimeSpan.Zero
        };
    }

    public TokenValidationParameters RefreshValidationParameters()
    {
        var secret = _configuration[JwtSettings.Element.Secret];
        return new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateLifetime = false,
        };
    }
}
