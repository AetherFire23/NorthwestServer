using IntegrationTests.Utils;

namespace IntegrationTests.Players;

public class PlayerInfo
{
    public RegisterRequest RegisterRequest { get; set; }
    public Store Store { get; set; }
    public SwagClient Client;
    public PlayerInfo(HttpClient client, RegisterRequest registerRequest)
    {
        Client = client.ToNSwagClient();
        RegisterRequest = registerRequest;
    }

    public async Task UpdateGameState()
    {
        var state = await Client.GameStateAsync(Store.GameState.PlayerUID, Store.GameState.TimeStamp);
        this.Store.GameState = state;
    }
}
