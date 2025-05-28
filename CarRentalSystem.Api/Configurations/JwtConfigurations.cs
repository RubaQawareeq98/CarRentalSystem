namespace CarRentalSystem.Api.Configurations;

public class JwtConfigurations
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int TokenExpirationMinutes { get; set; }
}
