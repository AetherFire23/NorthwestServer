using Microsoft.Extensions.DependencyInjection;
using Northwest.Domain.Authentication;
using Northwest.Domain.GameStart;
using Northwest.Domain.GameTasks;
using Northwest.Domain.Services;
using Northwest.Persistence;

namespace Northwest.Domain.Initialization;

internal static class DomainServicesConfigurator
{
    internal static void AddServices(IServiceCollection serviceCollection)
    {
        // Dependencies
        serviceCollection.AddScoped<CycleManagerService>();

        // Services
        serviceCollection.AddScoped<PlayerService>();
        serviceCollection.AddScoped<GameTaskService>();
        serviceCollection.AddScoped<PlayerService>();
        serviceCollection.AddScoped<GameStateService>();
        serviceCollection.AddScoped<ChatService>();
        serviceCollection.AddScoped<GameMakerService>();
        serviceCollection.AddScoped<ShipService>();
        serviceCollection.AddScoped<JoinLobbyService>();
        

        // Landmasses
        serviceCollection.AddScoped<UserService>();

        // Seeding
        serviceCollection.AddScoped<AuthenticationService>();
        serviceCollection.AddScoped<LobbyService>();
        serviceCollection.AddScoped<LobbyRepository>();

        serviceCollection.AddDbContext<PlayerContext>();
    }
}
