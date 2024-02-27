using IntegrationTests.GameStart;
using IntegrationTests.GameTask;
using IntegrationTests.Players;

namespace IntegrationTests.Services;

public static class TestServicesRegistration
{
    public static ServiceProvider RegisterTestServices()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.RegisterTestServices();
        ServiceProvider sp = serviceCollection.BuildServiceProvider();
        return sp;
    }

    public static void RegisterTestServices(this ServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<TestState>();
        serviceCollection.AddSingleton<RegistrationPhase>();
        serviceCollection.AddSingleton<LobbyPhase>();
        serviceCollection.AddSingleton<TestGameTasks>();
    }
}
