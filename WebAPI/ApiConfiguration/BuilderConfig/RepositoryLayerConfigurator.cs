using Northwest.Domain.Authentication;
using Northwest.Domain.Game_Actions;
using Northwest.Domain.Repositories;

namespace Northwest.WebApi.ApiConfiguration.BuilderConfig;

public class RepositoryLayerConfigurator
{
    public static void AddRepositoriesLayer(WebApplicationBuilder builder)
    {
        // Repos
        _ = builder.Services.AddScoped<GameRepository>();
        _ = builder.Services.AddScoped<MainMenuRepository>();
        _ = builder.Services.AddScoped<GameActionsRepository>();
        _ = builder.Services.AddScoped<RoomRepository>();
        _ = builder.Services.AddScoped<GameStateRepository>();
        _ = builder.Services.AddScoped<StationRepository>();
        _ = builder.Services.AddScoped<PlayerRepository>();
        _ = builder.Services.AddScoped<ShipRepository>();
        builder.Services.AddScoped<GameTaskAvailabilityRepository>();

        //landmasses
        _ = builder.Services.AddScoped<UserRepository>();
        _ = builder.Services.AddScoped<JwtTokenManager>();
    }
}
