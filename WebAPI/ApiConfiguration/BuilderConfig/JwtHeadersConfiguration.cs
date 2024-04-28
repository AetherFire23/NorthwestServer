using System.Text;
using Microsoft.IdentityModel.Tokens;
using Northwest.Domain.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace Northwest.WebApi.ApiConfiguration.BuilderConfig;

public static class JwtConfigReader
{
    public static IConfigurationSection GetJwtConfigSection(this ConfigurationManager configurationManager)
    {
        var jwtConfigSection = configurationManager.GetSection("Jwt");
        return jwtConfigSection;
    }
    public static JwtConfig ReadJwtConfig(ConfigurationManager configurationManager)
    {
        var section = configurationManager.GetJwtConfigSection();
        var jwtConfig = new JwtConfig();
        section.Bind(jwtConfig);

        return jwtConfig;
    }

    public static void RegisterJwtConfigOptions(this WebApplicationBuilder builder)
    {
        var section = builder.Configuration.GetJwtConfigSection();
        builder.Services.Configure<JwtConfig>(section);
        //builder.Services.Configure<JwtConfig>(r =>
        //{
        //    r.Issuer = "sd";
        //});
    }
}

public static class JwtHeadersConfiguration
{
    public static void ConfigureJWTHeaders(this WebApplicationBuilder builder)
    {
        builder.RegisterJwtConfigOptions();

        var jwtConfig = JwtConfigReader.ReadJwtConfig(builder.Configuration);

        builder.Services.AddAuthentication(options =>
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
