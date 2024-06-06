using DomainTests.UserSession;
using Northwest.Domain.Models;
using Northwest.Domain.Services;
using Northwest.Persistence.Entities;
using SharedUtils.Extensions;
namespace DomainTests.GameStart;

public class GameStartService(UserFactory userFactory, LobbyService lobbyService, GlobalTestState globalTestState)
{
    private readonly UserFactory _userFactory = userFactory;
    private readonly LobbyService _lobbyService = lobbyService;
    private readonly GlobalTestState _globalTestState = globalTestState;

    public async Task CreateUsersAndStartGame()
    {
        var generated = await RegisterUsers();

        var lobbyId = await CreateInitialLobby(_globalTestState.LocalUserId);
        int i = 0;
        await JoinLobbyForAllPlayers(_globalTestState.ClientAppStates, lobbyId);
    }

    private async Task<List<ClientAppState>> RegisterUsers()
    {
        //var users = new List<ClientAppState>();
        //foreach (var user in Enumerable.Range(0, 5))
        //{
        //    var created = await _userFactory.CreateUser();
        //    users.Add(created);
        //}

        var users = await Enumerable
            .Range(0, 5)
            .SelectAsync(async x => await _userFactory.CreateUser());

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
        // localPlayer should always be the same so that react can Auto-register
        // should filter the localState player 
        foreach (var client in clientAppStates.Where(x => x.UserId != _globalTestState.LocalUserId))
        {
            await _lobbyService.JoinLobby(new JoinLobbyRequest()
            {
                LobbyId = lobbyId,
                UserId = client.UserId,
            });
        }
    }
}