using Microsoft.EntityFrameworkCore;
using Northwest.Domain.Dtos;
using Northwest.Persistence;
using Northwest.Persistence.Entities;
using SharedUtils.Extensions;
namespace Northwest.Domain.GameStart;

public class LobbyRepository(PlayerContext playerContext)
{
    private const int _maximumPlayersIngame = 8;

    public async Task AddLobby(Lobby lobby)
    {
        playerContext.Add(lobby);
    }

    public async Task<Lobby?> GetLobbyById(Guid lobbyId)
    {
        var lobby = await playerContext.Lobbies
            .Include(p => p.UserLobbies)
                .ThenInclude(ur => ur.User)
            .FirstOrDefaultAsync(x => x.Id == lobbyId);

        return lobby;
    }

    public void RemoveLobby(Lobby entity)
    {
        playerContext.Lobbies.Remove(entity);
    }

    public async Task<LobbyDto> MapLobbyDto(Guid lobbyId)
    {
        var lobby = await GetLobbyById(lobbyId);
        var usersInLobby = lobby.UserLobbies.Select(ur => ur.User).ToList();
        var lobbyDto = new LobbyDto()
        {
            Id = lobbyId,
            UsersInLobby = usersInLobby,
        };

        return lobbyDto;
    }

    public async Task AddUserLobby(UserLobby userLobby)
    {
        await playerContext.UserLobbies.AddAsync(userLobby);
    }

    public async Task<Lobby> CreateAndAddLobby()
    {
        var newLobby = new Lobby() { Id = Guid.NewGuid() };
        await AddLobby(newLobby);
        await playerContext.SaveChangesAsync();
        return newLobby;
    }

    public async Task<bool> IsUserLobbyExists(Guid userId, Guid lobbyId) // pourrait avoir dequoi de plus generic 
    {
        bool userExists = await playerContext.UserLobbies
            .AnyAsync(x => x.User.Id == userId && x.Lobby.Id == lobbyId);
        return userExists;
    }

    public async Task<UserLobby?> FindUserLobby(Guid userId, Guid lobbyId)
    {
        var userLobby = await playerContext.UserLobbies
            .Include(ul => ul.User)
            .Include(ul => ul.Lobby)
            .FirstOrDefaultAsync(x => x.User.Id == userId && x.Lobby.Id == lobbyId);
        return userLobby;
    }

    public async Task DeleteUserFromLobby(Guid userId, Guid lobbyId)
    {
        var userLobby = await FindUserLobby(userId, lobbyId);
        playerContext.UserLobbies.Remove(userLobby);
        await playerContext.SaveChangesAsync();
    }

    public async Task DeleteLobbyIfEmpty(Lobby lobby)
    {
        if (!lobby.UsersInLobby.Any())
        {
            RemoveLobby(lobby);
            await playerContext.SaveChangesAsync();
        }
    }

    public async Task DeleteManyUsersFromLobby(List<User> users, Guid lobbyId)
    {
        foreach (var user in users)
        {
            await DeleteUserFromLobby(user.Id, lobbyId);
        }
    }

    //// moveto repository
    //public async Task CreateNewUserLobbyAndAddToDb(Guid userId, Guid lobbyId)
    //{
    //    await playerContext.SaveChangesAsync();

    //    var trackedUser = await _userRepository.GetUserById(userId);
    //    var trackedLobby = await GetLobbyById(lobbyId);
    //    var userLobby = new UserLobby()
    //    {
    //        User = trackedUser,
    //        Lobby = trackedLobby,
    //    };

    //    playerContext.UserLobbies.Add(userLobby);
    //    await playerContext.SaveChangesAsync();
    //}

    public async Task<List<LobbyDto>> GetLobbyDtosForUser(User user)
    {
        var lobbyDtos2 = await user.Lobbies.SequentialSelectAsync(l => MapLobbyDto(l.Id));
        return lobbyDtos2.ToList();
    }

    public async Task<bool> IsLobbyFull(Guid lobbyId)
    {
        var lobby = await GetLobbyById(lobbyId);
        bool isMaximumUsers = lobby.UsersInLobby.Count == _maximumPlayersIngame;
        return isMaximumUsers;
    }
}
