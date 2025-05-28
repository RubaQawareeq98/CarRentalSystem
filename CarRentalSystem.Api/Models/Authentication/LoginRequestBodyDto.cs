namespace CarRentalSystem.Api.Models.Authentication;

public class LoginRequestBodyDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
