using IntegrationTests.GameStart;
using IntegrationTests.GameTask;
using IntegrationTests.Players;
using IntegrationTests.Services;
using IntegrationTests.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Northwest.WebApi;
namespace IntegrationTests;

public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly ServiceProvider _serviceProvider;
    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _serviceProvider = TestServicesRegistration.RegisterTestServices();
    }

    [Fact]
    public async Task TestGame()
    {
        var registerPhase = _serviceProvider.GetRequiredService<RegistrationPhase>();
        var lobbyPhase = _serviceProvider.GetRequiredService<LobbyPhase>();

        var createClient = () => _factory.CreateClient().ToNSwagClient();

        await registerPhase.RegisterPlayers(createClient);
        await lobbyPhase.LobbyCreation();
        await lobbyPhase.GameStarts();

        var state = _serviceProvider.GetRequiredService<TestState>();
        var g = await state.LocalUserInfo.UpdateGameState();

        var testService = _serviceProvider.GetRequiredService<TestGameTasks>();
        await testService.DoTestTask();

        var g2 = await state.LocalUserInfo.UpdateGameState();
    }

    [Fact]
    public async Task TestGame2()
    {

    }
}
