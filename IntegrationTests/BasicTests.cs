using IntegrationTests.GameStart;
using IntegrationTests.Players;
using IntegrationTests.Services;
using IntegrationTests.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using WebAPI;
using Xunit.Abstractions;
namespace IntegrationTests;

public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    //private readonly SwagClient _client;
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
        await registerPhase.RegisterPlayers();
    }
}