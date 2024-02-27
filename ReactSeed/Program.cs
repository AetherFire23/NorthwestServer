
using IntegrationTests;
using IntegrationTests.GameStart;
using IntegrationTests.Players;
using IntegrationTests.Services;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    private static ServiceProvider _serviceProvider;

    static async Task Main(string[] args)
    {
        await Task.Delay(4000);
        _serviceProvider = TestServicesRegistration.RegisterTestServices();

        Func<SwagClient> createClient = () => new SwagClient("http://localhost:7060", new HttpClient());



        RegistrationPhase registerPhase = _serviceProvider.GetRequiredService<RegistrationPhase>();
        LobbyPhase lobbyPhase = _serviceProvider.GetRequiredService<LobbyPhase>();

        await registerPhase.RegisterPlayers(createClient);
        await lobbyPhase.LobbyCreation();
        await lobbyPhase.GameStarts();

        TestState state = _serviceProvider.GetRequiredService<TestState>();
        GameState g = await state.LocalUserInfo.UpdateGameState();

    }
}