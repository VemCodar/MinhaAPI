using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MinhaAPI.Models;

namespace MinhaAPI.Setttings;

public static class AuthSettings
{
    public static IServiceCollection AddAuthenticationJWT(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt =>
        {
            var authConfig = new AuthConfig(configuration);
            opt.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidIssuer = authConfig.Issuer,
                ValidAudience = authConfig.Audience, 
                ValidateIssuer = true,
                ValidateAudience = true,
                IssuerSigningKey = new SymmetricSecurityKey(authConfig.CreateSecretKeyBytes())
            };
            //É importante que seja true em PRD
            opt.RequireHttpsMetadata = false; 
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("admin", p => p.RequireRole("admin"));
            options.AddPolicy("sudo", p => p.RequireRole("sudo"));
        });

        return services;
    }
}