﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Northwest.Domain.Authentication;
using System.Text;

namespace Northwest.WebApi.ApiConfiguration.BuilderConfig;

public static class JwtHeadersConfiguration
{

    // Must be executed before
    // _ = builder.Services.AddAuthorization();
    public static void ConfigureJWTHeaders(this WebApplicationBuilder builder)
    {
        IConfigurationSection jwtSection = builder.Configuration.GetSection("Jwt");
        JwtConfig jwtConfig = new JwtConfig();
        jwtSection.Bind(jwtConfig);
        _ = builder.Services.Configure<JwtConfig>(jwtSection);

        // Configure JWT authentication
        _ = builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            // Set JWT bearer options
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidAudience = jwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
            };
        });

    }
}
