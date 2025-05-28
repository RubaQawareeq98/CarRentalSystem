namespace CarRentalSystem.Api.Configurations;

public class ElasticSearchConfigurations
{
    public string ElasticSearchUri { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string IndexFormat { get; set; } = string.Empty;
}