namespace RestaurantReservation.API.Models.Authentication;

public class LoginRequestBodyDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
