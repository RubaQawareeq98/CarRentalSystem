using CarRentalSystem.Api.Configurations;

namespace CarRentalSystem.Api.ServiceRegistration;

public static class AddJwtParams
{
    public static void RegisterJwtParams(this WebApplicationBuilder builder)
    {
        var jwtSection = builder.Configuration.GetSection("Authentication");

        builder.Services.Configure<JwtConfiguration>(jwtSection);

        var jwtConfig = jwtSection.Get<JwtConfiguration>();
        ArgumentNullException.ThrowIfNull(jwtConfig);
        
        builder.Services.AddSingleton(jwtConfig);
        
        builder.Services.RegisterAuthentication(jwtConfig);
    }
}
