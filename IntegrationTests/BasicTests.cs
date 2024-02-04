using Microsoft.AspNetCore.Mvc.Testing;
using WebAPI;
namespace IntegrationTests;

public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Client _client;
    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        var factoriedClient = _factory.CreateClient();
        var swagClient = new Client("/", factoriedClient);
        _client = swagClient;
    }

    [Fact]
    public void Test1()
    {
        var registerRequest = new RegisterRequest
        {
            Email = "testmaster@proton.exe",
            Password = "password",
            UserName = "username",
        };

        _client.RegisterAsync(registerRequest);
    }
}