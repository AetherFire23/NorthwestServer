using IntegrationTests.Players;
using IntegrationTests.Utils;
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
        UserInfo localPlayer = await RegisterNewPlayer(createClient, true);
        Console.WriteLine($"username: {localPlayer.RegisterRequest.UserName}");
        Console.WriteLine($"password: {localPlayer.RegisterRequest.Password}");

        List<UserInfo> otherPlayers = await RegisterOtherPlayers(createClient);
        _state.LocalUserInfo = localPlayer;
        _state.OtherPlayersInfos = otherPlayers.ToList();
    }
    private async Task<List<UserInfo>> RegisterOtherPlayers(Func<SwagClient> createClient)
    {
        List<UserInfo> otherPlayers = new List<UserInfo>();
        for (int i = 0; i < 5; i++)
        {
            UserInfo p = await RegisterNewPlayer(createClient, false);
            otherPlayers.Add(p);
        }

        return otherPlayers;
    }
    private async Task<UserInfo> RegisterNewPlayer(Func<SwagClient> createClient, bool isDefaultRequest)
    {
        string userName = Generation.CreateRandomUserName();

        RegisterRequest registerRequest = isDefaultRequest ? DefaultRequest.DefaultLocalPlayerRequest : new RegisterRequest
        {
            UserName = userName,
            Email = Generation.GenerateEmail(userName),
            Password = Guid.NewGuid().ToString(),
        };

        UserInfo playerInfo = new UserInfo(createClient(), registerRequest)
        {
            RegisterRequest = registerRequest,
        };

        UserDto userDto = await playerInfo.Client.RegisterAsync(registerRequest);

        LoginRequest loginRequest = new LoginRequest()
        {
            PasswordAttempt = registerRequest.Password,
            UserName = registerRequest.UserName,
        };
        LoginResult loginResult = await playerInfo.Client.LoginAsync(loginRequest);

        MainMenuState mainMenuState = await playerInfo.Client.GetMainMenuStateAsync(loginResult.UserId);

        Store store = new Store()
        {
            GameState = new GameState(),
            MainMenuState = mainMenuState,
        };

        playerInfo.Store = store;

        return playerInfo;
    }
}
