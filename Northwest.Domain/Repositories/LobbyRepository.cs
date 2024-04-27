using Microsoft.EntityFrameworkCore;
using Northwest.Domain.Dtos;
using Northwest.Persistence;
using Northwest.Persistence.Entities;

namespace Northwest.Domain.Repositories;

public class LobbyRepository
{
    private readonly PlayerContext _playerContext;
    public LobbyRepository(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    //public async Task AddLobby(Lobby lobber) => await AddEntity(lobber);

    public async Task AddLobby(Lobby lobby)
    {
        _playerContext.Add(lobby);
    }
    public async Task<Lobby?> GetLobbyById(Guid lobbyId)
    {
        Lobby? lobby = await _playerContext.Lobbies
            .Include(p => p.UserLobbies)
                .ThenInclude(ur => ur.User)
            .FirstOrDefaultAsync(x => x.Id == lobbyId);
        return lobby;
    }


    public void RemoveLobby(Lobby entity)
    {
        _playerContext.Lobbies.Remove(entity);
    }

    public async Task<LobbyDto> MapLobbyDto(Guid lobbyId)
    {
        Lobby? lobby = await GetLobbyById(lobbyId);
        List<User> usersInLobby = lobby.UserLobbies.Select(ur => ur.User).ToList();
        LobbyDto lobbyDto = new LobbyDto()
        {
            Id = lobbyId,
            UsersInLobby = usersInLobby,
        };
        return lobbyDto;
    }

    public async Task AddUserLobby(UserLobby userLobby)
    {
        await _playerContext.UserLobbies.AddAsync(userLobby);
    }

    public async Task<Lobby> CreateAndAddLobby()
    {
        Lobby newLobby = new Lobby() { Id = Guid.NewGuid() };
        await AddLobby(newLobby);
        await _playerContext.SaveChangesAsync();
        return newLobby;
    }

    public async Task<bool> IsUserLobbyAlreadyExists(Guid userId, Guid lobbyId) // pourrait avoir dequoi de plus generic 
    {
        bool userExists = await _playerContext.UserLobbies
            .AnyAsync(x => x.User.Id == userId && x.Lobby.Id == lobbyId);
        return userExists;
    }

    public async Task<UserLobby?> FindUserLobby(Guid userId, Guid lobbyId)
    {
        UserLobby? userLobby = await _playerContext.UserLobbies
            .Include(ul => ul.User)
            .Include(ul => ul.Lobby)
            .FirstOrDefaultAsync(x => x.User.Id == userId && x.Lobby.Id == lobbyId);
        return userLobby;
    }

    public async Task DeleteUserFromLobby(Guid userId, Guid lobbyId)
    {
        UserLobby? userLobby = await FindUserLobby(userId, lobbyId);
        _playerContext.UserLobbies.Remove(userLobby);
        await _playerContext.SaveChangesAsync();
    }

    public async Task DeleteLobbyIfEmpty(Lobby lobby)
    {
        if (!lobby.UsersInLobby.Any())
        {
            RemoveLobby(lobby);
            _ = await _playerContext.SaveChangesAsync();
        }
    }

    public async Task DeleteManyUsersFromLobby(List<User> users, Guid lobbyId)
    {
        foreach (User user in users)
        {
            await DeleteUserFromLobby(user.Id, lobbyId);
        }
    }
}
