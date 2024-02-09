using Microsoft.EntityFrameworkCore;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;

namespace WebAPI.Repositories;

public class LobbyRepository : RepositoryBase<Lobby, PlayerContext>
{
    public LobbyRepository(PlayerContext playerContext
        ) : base(playerContext)
    {
    }

    public async Task AddLobby(Lobby lobber) => await AddEntity(lobber);

    public async Task<Lobby?> GetLobbyById(Guid lobbyId)
    {
        Lobby? lobby = await Set
            .Include(p => p.UserLobbies)
            .ThenInclude(ur => ur.User)
            .FirstOrDefaultAsync(x => x.Id == lobbyId);
        return lobby;
    }


    public void RemoveLobby(Lobby entity)
    {
        _ = Set.Remove(entity);
    }

    public async Task<LobbyDto> MapLobbyDto(Guid lobbyId)
    {
        Lobby? lobby = await GetLobbyById(lobbyId);
        List<User> usersInLobby = lobby.UserLobbies.Select(ur => ur.User).ToList();
        LobbyDto lobbyDto = new LobbyDto()
        {
            Id = lobbyId,
            QueuingUsers = usersInLobby,
        };
        return lobbyDto;
    }

    public async Task AddUserLobby(UserLobby userLobby)
    {
        _ = await Context.UserLobbies.AddAsync(userLobby);
    }

    public async Task<Lobby> CreateAndAddLobby()
    {
        Lobby newLobby = new Lobby() { Id = Guid.NewGuid() };
        await AddLobby(newLobby);
        _ = await Context.SaveChangesAsync();
        return newLobby;
    }

    public async Task<bool> IsUserLobbyAlreadyExists(Guid userId, Guid lobbyId) // pourrait avoir dequoi de plus generic 
    {
        bool userExists = await Context.UserLobbies
            .AnyAsync(x => x.User.Id == userId && x.Lobby.Id == lobbyId);
        return userExists;
    }

    public async Task<UserLobby?> GetUserLobbyByJoinTargetIds(Guid userId, Guid lobbyId)
    {
        UserLobby? userLobby = await Context.UserLobbies
            .Include(ul => ul.User)
            .Include(ul => ul.Lobby)
            .FirstOrDefaultAsync(x => x.User.Id == userId && x.Lobby.Id == lobbyId);
        return userLobby;
    }

    public async Task DeleteUserFromLobby(Guid userId, Guid lobbyId)
    {
        UserLobby? userLobby = await GetUserLobbyByJoinTargetIds(userId, lobbyId);
        _ = Context.UserLobbies.Remove(userLobby);
        _ = await Context.SaveChangesAsync();
    }

    public async Task DeleteLobbyIfEmpty(Lobby lobby)
    {
        if (!lobby.UsersInLobby.Any())
        {
            RemoveLobby(lobby);
            _ = await Context.SaveChangesAsync();
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
