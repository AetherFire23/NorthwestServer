
using Microsoft.Extensions.DependencyInjection;
using Northwest.Domain.Authentication;
using Northwest.Domain.Game_Actions;
using Northwest.Domain.Repositories;

namespace Northwest.Domain.Initialization;

internal static class DomainRepositoryConfigurator
{

    internal static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<GameRepository>();
        serviceCollection.AddScoped<MainMenuRepository>();
        serviceCollection.AddScoped<GameActionsRepository>();
        serviceCollection.AddScoped<RoomRepository>();
        serviceCollection.AddScoped<GameStateRepository>();
        serviceCollection.AddScoped<StationRepository>();
        serviceCollection.AddScoped<PlayerRepository>();
        serviceCollection.AddScoped<ShipRepository>();
        serviceCollection.AddScoped<GameTaskAvailabilityRepository>();

        // Landmasses
        serviceCollection.AddScoped<UserRepository>();
        serviceCollection.AddScoped<JwtTokenManager>();
    }
}