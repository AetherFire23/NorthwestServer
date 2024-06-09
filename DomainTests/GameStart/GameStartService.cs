using Northwest.Domain.GameStart;
using Northwest.Domain.Initialization.GameOptions;
using Northwest.Domain.Models;
using DomainTests.UserSession;
using SharedUtils.Extensions;
namespace DomainTests.GameStart;

public class GameStartService(UserFactory _userFactory, LobbyService _lobbyService, GlobalTestState _globalTestState)
{
    public async Task CreateUsersAndStartGame()
    {
        var generated = await RegisterUsers();
        var lobbyId = await CreateInitialLobby(_globalTestState.LocalUserId);
        await JoinLobbyForAllPlayers(_globalTestState.ClientAppStates, lobbyId);

        await _lobbyService.StartGame(lobbyId);
    }

    private async Task<List<ClientAppState>> RegisterUsers()
    {
        var users = await Enumerable
            .Range(0, GameOptionsData.MaximumPlayerAmount) // Creates the exact amount of users I need 
            .SequentialSelectAsync(async x => await _userFactory.CreateUser());

        _globalTestState.ClientAppStates = users.ToList();
        _globalTestState.LocalPlayerState = users.First();

        return users.ToList();
    }

    private async Task<Guid> CreateInitialLobby(Guid localUserId)
    {
        var lobby = await _lobbyService.CreateAndJoinLobby(localUserId);
        return lobby.Id;
    }

    private async Task JoinLobbyForAllPlayers(List<ClientAppState> clientAppStates, Guid lobbyId)
    {
        var otherAppStates = clientAppStates.Where(x => x.UserId != _globalTestState.LocalUserId);

        foreach (var client in otherAppStates)
        {
            await _lobbyService.JoinLobby(new JoinLobbyRequest
            {
                LobbyId = lobbyId,
                UserId = client.UserId,
            });
        }
    }

}