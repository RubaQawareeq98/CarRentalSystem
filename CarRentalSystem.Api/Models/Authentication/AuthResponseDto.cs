namespace CarRentalSystem.Api.Models.Authentication;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public int ExpirationInMinutes { get; set; }
}
