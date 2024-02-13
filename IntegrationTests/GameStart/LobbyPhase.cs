using IntegrationTests.Players;

namespace IntegrationTests.GameStart;

public class LobbyPhase
{
    private readonly TestState _state;
    private Guid _createdLobbyId = Guid.Empty;
    public LobbyPhase(TestState state)
    {
        _state = state;
    }

    public async Task LobbyCreation()
    {
        // local player creates lobby
        var lobby = await _state.LocalUserInfo.Client.CreateLobbyAsync(_state.LocalUserInfo.Id);
        _createdLobbyId = lobby.Id;

        // all other players join that lobby
        foreach (var userInfo in _state.OtherPlayersInfos)
        {
            await userInfo.UpdateMainMenuState();
            var joinRequest = new JoinLobbyRequest
            {
                LobbyId = userInfo.Store.MainMenuState.UserDto.AvailableLobbies.First().Id,
                UserId = userInfo.Id,
            };
            await userInfo.Client.JoinLobbyAsync(joinRequest);
        }
    }

    public async Task GameStarts()
    {
        await _state.LocalUserInfo.UpdateMainMenuState();
        await _state.LocalUserInfo.Client.StartGameAsync(_createdLobbyId);

    }
}
