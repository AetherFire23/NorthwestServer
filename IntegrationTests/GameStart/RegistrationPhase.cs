using IntegrationTests.Players;
using IntegrationTests.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using WebAPI;

namespace IntegrationTests.GameStart;

public class RegistrationPhase
{
    private readonly TestState _state;
    private readonly WebApplicationFactory<Program> _webApplicationFactory;
    public RegistrationPhase(TestState state, WebApplicationFactory<Program> webApplicationFactory)
    {
        _state = state;
        _webApplicationFactory = webApplicationFactory;
    }


    public async Task RegisterPlayers()
    {
        var localPlayer = await RegisterNewPlayer();
        var otherPlayers = await Task.WhenAll(Enumerable.Range(1, 5).Select(async x => await RegisterNewPlayer()));
        _state.LocalPlayerInfo = localPlayer;
        _state.OtherPlayersInfos = otherPlayers.ToList();
    }

    private async Task<PlayerInfo> RegisterNewPlayer()
    {
        var userName = Generation.CreateRandomUserName();

        var registerRequest = new RegisterRequest
        {
            UserName = userName,
            Email = Generation.GenerateEmail(userName),
            Password = Guid.NewGuid().ToString(),
        };

        var playerInfo = new PlayerInfo(_webApplicationFactory.CreateClient(), registerRequest)
        {
            RegisterRequest = registerRequest,
        };

        var userDto = await playerInfo.Client.RegisterAsync(registerRequest);

        var loginRequest = new LoginRequest()
        {
            PasswordAttempt = registerRequest.Password,
            UserName = registerRequest.UserName,
        };
        var loginResult = await playerInfo.Client.LoginAsync(loginRequest);

        var mainMenuState = await playerInfo.Client.GetMainMenuStateAsync(loginResult.UserId);

        var store = new Store()
        {
            GameState = new GameState(),
            MainMenuState = mainMenuState,
        };

        playerInfo.Store = store;

        return playerInfo;
    }
}
