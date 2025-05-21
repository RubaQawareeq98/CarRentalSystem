namespace CarRentalSystem.Db.Models;

public class CarSearchDto
{
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? Location { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MinYear { get; set; }
    public int? MaxYear { get; set; }
}
