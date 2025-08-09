namespace WebApiDotNet.Data
{
    public sealed class JwtSettings
    {
        public string Key { get; set; } = string.Empty;
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int AccessTokenMinutes { get; set; } = 60;
        public int RefreshTokenDays { get; set; } = 30;
        public string AccessTokenCookieName { get; set; } = "AuthToken";
        public string RefreshTokenCookieName { get; set; } = "RefreshToken";
    }
}
