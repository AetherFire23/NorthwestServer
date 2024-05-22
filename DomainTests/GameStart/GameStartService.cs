using DomainTests.UserSession;
using Northwest.Domain.Models;
using Northwest.Domain.Services;
namespace DomainTests.GameStart;

public class GameStartService(UserFactory userFactory, LobbyService lobbyService, GlobalTestState globalTestState)
{
    private readonly UserFactory _userFactory = userFactory;
    private readonly LobbyService _lobbyService = lobbyService;
    private readonly GlobalTestState _globalTestState = globalTestState;

    public async Task CreateUsersAndStartGame()
    {
        var generated = await RegisterUsers() ;

        var lobbyId = await CreateInitialLobby(_globalTestState.LocalUserId);
    }

    private async Task<List<ClientAppState>> RegisterUsers()
    {
        var users = new List<ClientAppState>();
        foreach (var user in Enumerable.Range(0,5))
        {
            var created = await _userFactory.CreateUser();
            users.Add(created);
        }

        _globalTestState.UserStates = users.ToList();
        
        return users.ToList();
    }

    private async Task<Guid> CreateInitialLobby(Guid localUserId)
    {
        var lobby =  await _lobbyService.CreateAndJoinLobby(localUserId);

        return lobby.Id;
    }

    private async void JoinLobbyForAllPlayers(List<ClientAppState> clientAppStates, Guid lobbyId)
    {
        foreach (var client in clientAppStates)
        {
            await _lobbyService.JoinLobby(new JoinLobbyRequest()
            {
                LobbyId = lobbyId,
                UserId = client.UserId,
            });
        }
    }
}