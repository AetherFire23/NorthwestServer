using Microsoft.AspNetCore.Mvc.Testing;
using WebAPI;
namespace IntegrationTests;

public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Client _client;
    private UserDto _userDto;
    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        var factoriedClient = _factory.CreateClient();
        var swagClient = new Client("/", factoriedClient);
        _client = swagClient;

        var s = new ServiceCollection();
        var sp = s.BuildServiceProvider();
    }

    // will actually have to code a list of the players 
    [Fact]
    public async Task Test1()
    {
        await Register();
        var loginRequest = new LoginRequest()
        {
            PasswordAttempt = "password",
            UserName = "username",
        };

        var loginResult = await _client.LoginAsync(loginRequest);
    }

    private async Task Register()
    {
        RegisterRequest registerRequest = new RegisterRequest
        {
            Email = "testmaster@proton.exe",
            Password = "password",
            UserName = "username",
        };

        _userDto = await _client.RegisterAsync(registerRequest);
    }

    // will eventually need real unit tests to test specific tasks
    [Fact]
    private async Task Testoman()
    {

    }
}