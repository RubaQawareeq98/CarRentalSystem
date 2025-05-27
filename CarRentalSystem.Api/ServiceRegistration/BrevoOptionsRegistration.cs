using CarRentalSystem.Api.Configurations;

namespace CarRentalSystem.Api.ServiceRegistration;

public static class BrevoOptionsRegistration
{
    public static void RegisterBrevoOptions(this WebApplicationBuilder builder)
    {
        var brevoSection = builder.Configuration.GetSection("BrevoSettings");

        builder.Services.Configure<BrevoSettings>(brevoSection);

        var brevoConfig = brevoSection.Get<BrevoSettings>();
        ArgumentNullException.ThrowIfNull(brevoConfig);
        
        builder.Services.AddSingleton(brevoConfig);
    }
}