using IntegrationTests.Players;
using IntegrationTests.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using WebAPI;
namespace IntegrationTests.GameStart;

public class RegistrationPhase
{
    private readonly TestState _state;
    public RegistrationPhase(TestState state)
    {
        _state = state;
    }
    public async Task RegisterPlayers(Func<SwagClient> createClient)
    {
        var localPlayer = await RegisterNewPlayer(createClient, true);
        Console.WriteLine($"username: {localPlayer.RegisterRequest.UserName}");
        Console.WriteLine($"password: {localPlayer.RegisterRequest.Password}");

        var otherPlayers = await RegisterOtherPlayers(createClient);
        _state.LocalUserInfo = localPlayer;
        _state.OtherPlayersInfos = otherPlayers.ToList();
    }
    private async Task<List<UserInfo>> RegisterOtherPlayers(Func<SwagClient> createClient)
    {
        var otherPlayers = new List<UserInfo>();
        for (int i = 0; i < 5; i++)
        {
            var p = await RegisterNewPlayer(createClient, false);
            otherPlayers.Add(p);
        }

        return otherPlayers;
    }
    private async Task<UserInfo> RegisterNewPlayer(Func<SwagClient> createClient, bool isDefaultRequest)
    {
        var userName = Generation.CreateRandomUserName();

        var registerRequest = isDefaultRequest ? DefaultRequest.DefaultLocalPlayerRequest : new RegisterRequest
        {
            UserName = userName,
            Email = Generation.GenerateEmail(userName),
            Password = Guid.NewGuid().ToString(),
        };

        var playerInfo = new UserInfo(createClient(), registerRequest)
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
