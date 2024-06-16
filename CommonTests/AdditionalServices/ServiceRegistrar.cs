using DomainTests.GameStart;
using DomainTests.UserSession;
using Microsoft.Extensions.DependencyInjection;

namespace DomainTests.AdditionalServices;

public static class ServiceRegistrar
{
    public static void RegisterTestServices(this IServiceCollection serviceCollection)
    {
        // Test services specific to 
        serviceCollection.AddSingleton<GlobalTestState>();
        serviceCollection.AddScoped<TestEnvironmentStatesUpdater>();
        serviceCollection.AddScoped<GameStartService>();
        serviceCollection.AddScoped<UserFactory>();
    }
}