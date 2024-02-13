using IntegrationTests.GameStart;
using IntegrationTests.Players;
using IntegrationTests.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using WebAPI;
using Xunit.Abstractions;
namespace IntegrationTests;

public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;
    private readonly ServiceProvider _serviceProvider;
    public BasicTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
        _serviceProvider = TestServicesRegistration.GenerateTestServiceProvider(factory);
    }

    // will actually have to code a list of the players 
    [Fact]
    public async Task TestGame()
    {
        var registerPhase = _serviceProvider.GetRequiredService<RegistrationPhase>();
        var lobbyPhase = _serviceProvider.GetRequiredService<LobbyPhase>();
        await registerPhase.RegisterPlayers();
        await lobbyPhase.LobbyCreation();
        await lobbyPhase.GameStarts();

        var state = _serviceProvider.GetRequiredService<TestState>();


        var g = await state.LocalUserInfo.UpdateGameState();

    }
}