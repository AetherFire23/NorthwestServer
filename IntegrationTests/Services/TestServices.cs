using IntegrationTests.GameStart;
using IntegrationTests.Players;
using Microsoft.AspNetCore.Mvc.Testing;
using WebAPI;

namespace IntegrationTests.Services;

public static class TestServicesRegistration
{
    public static ServiceProvider GenerateTestServiceProvider(WebApplicationFactory<Program> webAppFactory)
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.RegisterTestServices(webAppFactory);

        var sp = serviceCollection.BuildServiceProvider();
        return sp;
    }

    public static void RegisterTestServices(this ServiceCollection serviceCollection, WebApplicationFactory<Program> webAppFactory)
    {
        serviceCollection.AddSingleton<TestState>();
        serviceCollection.AddSingleton<RegistrationPhase>();
        serviceCollection.AddSingleton(webAppFactory);
    }
}
