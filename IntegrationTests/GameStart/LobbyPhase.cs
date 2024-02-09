using IntegrationTests.Players;

namespace IntegrationTests.GameStart;

public class LobbyPhase
{
    private readonly TestState _state;

    public LobbyPhase(TestState state)
    {
        _state = state;
    }

    public async Task CreateLobby()
    {
        await _state.LocalPlayerInfo.Client.CreateLobbyAsync();
    }
}
