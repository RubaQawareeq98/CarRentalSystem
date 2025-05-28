namespace CarRentalSystem.Db.Models;

public class PasswordResetToken
{
    public int Id { get; set; }
    public string Email { get; set; } = String.Empty;
    public string Token { get; set; } = String.Empty;
    public DateTime ExpiryDate { get; set; }
}
