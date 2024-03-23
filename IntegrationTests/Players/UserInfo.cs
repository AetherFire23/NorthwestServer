namespace IntegrationTests.Players;

public class UserInfo
{
    public RegisterRequest RegisterRequest { get; set; }
    public Store Store { get; set; }
    public SwagClient Client;
    public Guid PlayerUID => Store.GameState.PlayerUID;
    public Guid Id => Store.MainMenuState.UserDto.Id;
    public UserInfo(SwagClient client, RegisterRequest registerRequest)
    {
        Client = client;
        RegisterRequest = registerRequest;
    }

    public async Task<GameState> UpdateGameState()
    {
        await UpdateMainMenuState();
        Guid gameId = Store.MainMenuState.UserDto.ActiveGames.First().Id;
        Guid playerId = Store.MainMenuState.UserDto.Players.First(x => x.GameId == gameId).Id;
        GameState state = await Client.GetGameStateAsync(playerId, Store.GameState.TimeStamp);
        this.Store.GameState = state;
        return state;
    }

    public async Task UpdateMainMenuState()
    {
        MainMenuState mainMenuState = await Client.GetMainMenuStateAsync(this.Id);
        Store.MainMenuState = mainMenuState;
    }
}
