
using IntegrationTests;
using IntegrationTests.GameStart;
using IntegrationTests.Players;
using IntegrationTests.Services;
using IntegrationTests.Utils;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    private static ServiceProvider _serviceProvider;

    static async Task Main(string[] args)
    {
        await Task.Delay(4000);
        _serviceProvider = TestServicesRegistration.GenerateTestServiceProvider();

        var createClient = () => new SwagClient("http://localhost:7060", new HttpClient());



        var registerPhase = _serviceProvider.GetRequiredService<RegistrationPhase>();
        var lobbyPhase = _serviceProvider.GetRequiredService<LobbyPhase>();

        await registerPhase.RegisterPlayers(createClient);
        await lobbyPhase.LobbyCreation();
        await lobbyPhase.GameStarts();

        var state = _serviceProvider.GetRequiredService<TestState>();
        var g = await state.LocalUserInfo.UpdateGameState();

    }
}