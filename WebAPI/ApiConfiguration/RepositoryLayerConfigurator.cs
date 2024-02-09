using WebAPI.Authentication;
using WebAPI.Game_Actions;
using WebAPI.Repositories;

namespace WebAPI.ApiConfiguration;

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
        _ = builder.Services.AddScoped<LandmassRepository>();
        _ = builder.Services.AddScoped<ShipRepository>();

        //landmasses
        _ = builder.Services.AddScoped<LandmassCardsRepository>();
        _ = builder.Services.AddScoped<UserRepository>();
        _ = builder.Services.AddScoped<JwtTokenManager>();
    }
}
