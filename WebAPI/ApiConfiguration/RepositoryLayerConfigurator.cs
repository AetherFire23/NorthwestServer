using WebAPI.Authentication;
using WebAPI.Game_Actions;
using WebAPI.Interfaces;
using WebAPI.Repository.Users;
using WebAPI.Repository;

namespace WebAPI.ApiConfiguration;

public class RepositoryLayerConfigurator
{
    public static void AddRepositoriesLayer(WebApplicationBuilder builder)
    {
        // Repos
        builder.Services.AddScoped<IGameRepository, GameRepository>();
        builder.Services.AddScoped<IMainMenuRepository, MainMenuRepository>();
        builder.Services.AddScoped<IGameActionsRepository, GameActionsRepository>();
        builder.Services.AddScoped<IRoomRepository, RoomRepository>();
        builder.Services.AddScoped<IGameStateRepository, GameStateRepository>();
        builder.Services.AddScoped<IStationRepository, StationRepository>();
        builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
        builder.Services.AddScoped<IChatRepository, ChatRepository>();
        builder.Services.AddScoped<IGameMakerRepository, GameMakerRepository>();
        builder.Services.AddScoped<ILandmassRepository, LandmassRepository>();
        builder.Services.AddScoped<IShipRepository, ShipRepository>();

        //landmasses
        builder.Services.AddScoped<ILandmassCardsRepository, LandmassCardsRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddScoped<IJwtTokenManager, JwtTokenManager>();
    }
}
