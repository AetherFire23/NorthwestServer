using IntegrationTests.Utils;

namespace IntegrationTests.Players;

public class UserInfo
{
    public RegisterRequest RegisterRequest { get; set; }
    public Store Store { get; set; }
    public SwagClient Client;
    public Guid PlayerUID => Store.GameState.PlayerUID;
    public Guid Id => Store.MainMenuState.UserDto.Id;
    public UserInfo(HttpClient client, RegisterRequest registerRequest)
    {
        Client = client.ToNSwagClient();
        RegisterRequest = registerRequest;
    }

    public async Task<GameState> UpdateGameState()
    {
        await UpdateMainMenuState();
        var gameId = Store.MainMenuState.UserDto.ActiveGames.First().Id;
        var playerId = Store.MainMenuState.UserDto.Players.First(x=> x.GameId == gameId).Id;
        var state = await Client.GameStateAsync(playerId, Store.GameState.TimeStamp);
        this.Store.GameState = state;
        return state;
    }

    public async Task UpdateMainMenuState()
    {
        var mainMenuState = await Client.GetMainMenuStateAsync(this.Id);
        Store.MainMenuState = mainMenuState;
    }
}
