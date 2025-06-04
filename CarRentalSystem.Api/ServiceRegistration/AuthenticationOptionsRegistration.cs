using CarRentalSystem.Api.Configurations;
using Microsoft.IdentityModel.Tokens;

namespace CarRentalSystem.Api.ServiceRegistration;

public static class AuthenticationOptionsRegistration
{
    public static void RegisterAuthentication(this IServiceCollection services, JwtConfigurations jwtConfig)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Convert.FromBase64String(jwtConfig.SecretKey)
                    )
                };
            });
    }
}
