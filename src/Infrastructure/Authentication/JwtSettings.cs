namespace Onion.Infrastructure.Authentication;

internal sealed class JwtSettings
{
    internal static class Element
    {
        public const string Secret = "JwtSettings:Secret";
        public const string Issuer = "JwtSettings:Issuer";
        public const string Audience = "JwtSettings:Audience";
    }

    public const string SectionName = "JwtSettings";

    public string Secret { get; init; } = null!;
    public TimeSpan TokenLifeTime { get; init; }
    public TimeSpan RefreshLifeTime { get; init; }
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
}
