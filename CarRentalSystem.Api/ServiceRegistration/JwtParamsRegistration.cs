using CarRentalSystem.Api.Configurations;

namespace CarRentalSystem.Api.ServiceRegistration;

public static class JwtParamsRegistration
{
    public static void RegisterJwtParams(this WebApplicationBuilder builder)
    {
        var jwtSection = builder.Configuration.GetSection("Authentication");

        builder.Services.Configure<JwtConfigurations>(jwtSection);

        var jwtConfig = jwtSection.Get<JwtConfigurations>();
        ArgumentNullException.ThrowIfNull(jwtConfig);
        
        builder.Services.AddSingleton(jwtConfig);
        
        builder.Services.RegisterAuthentication(jwtConfig);
    }
}
