using Northwest.Domain.Authentication;
using Northwest.Domain.Models.Requests;
using Northwest.Domain.Repositories;
using SharedUtils.Extensions;
namespace DomainTests.UserSession;

public class UserFactory
{
    private readonly AuthenticationService _authenticationService;
    private readonly MainMenuRepository _mainMenuRepository;
    private readonly GameStateRepository _gameStateRepository;

    public UserFactory(AuthenticationService authenticationService, MainMenuRepository mainMenuRepository, GameStateRepository gameStateRepository)
    {
        _authenticationService = authenticationService;
        _mainMenuRepository = mainMenuRepository;
        _gameStateRepository = gameStateRepository;
    }

    public async Task<ClientAppState> CreateUser()
    {
        // subscribe to the api
        var generatedCredentials = GenerateRandomRegisterRequest();
        var user = await _authenticationService.TryRegister(generatedCredentials);

        // get mainmenuState
        var mainMenuState = await _mainMenuRepository.GetMainMenuState(user.Id);

        var clientAppState = new ClientAppState()
        {
            MainMenuState = mainMenuState,
        };

        return clientAppState;
    }

    private RegisterRequest GenerateRandomRegisterRequest()
    {
        var username = NamesListReader.Read.Shuffle().Take(2);

        var username2 = string.Concat(username) + Guid.NewGuid().ToString().Skip(4);

        var registerRequest = new RegisterRequest()
        {
            Email = $"someEmail{username2}@hotmail.com",
            Password = "password",
            UserName = username2
        };

        return registerRequest;
    }
}