using Microsoft.EntityFrameworkCore;
using Northwest.Domain.Dtos;
using Northwest.Persistence;
using Northwest.Persistence.Entities;
using Northwest.Persistence.Enums;
public class PlayerRepository
{
    private readonly PlayerContext _playerContext;
    DbSet<Player> Players => _playerContext.Players;

    public PlayerRepository(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    public Player GetPlayer(string name)
    {
        Player? player = _playerContext.Players.FirstOrDefault(player => player.Name.Equals(name));

        if (player is null) throw new NotImplementedException("PlayerNotFound");

        return player;
    }

    public async Task<Player> GetPlayerAsync(Guid id)
    {
        Player player = await _playerContext.Players.FirstAsync(queriedPlayer => queriedPlayer.Id == id);
        return player;
    }

    public async Task<Item> GetItemAsync(Guid itemId)
    {
        Item item = await _playerContext.Items.FirstAsync(x => x.Id == itemId);

        return item;
    }

    public async Task<List<PrivateInvitation>> GetPlayerInvitations(Guid playerId)
    {
        List<PrivateInvitation> playerInvites = await _playerContext.Invitations.Where(invite => invite.ToPlayerId == playerId).ToListAsync();
        return playerInvites;
    }

    public async Task<List<Player>> GetPlayersInGameAsync(Guid gameId)
    {
        List<Player> allPlayers = await Players.Where(player => player.GameId == gameId).ToListAsync();
        return allPlayers;
    }

    public async Task<PlayerDto> MapPlayerDTOAsync(Guid playerId)
    {
        Player player = await GetPlayerAsync(playerId);

        List<Item> items = await GetOwnedItems(playerId);

        List<Skills> skillsOwned = GetOwnedSkills(playerId);

        PlayerDto playerDTO = new PlayerDto()
        {
            // Common with player
            Id = playerId,
            GameId = player.GameId,
            Name = player.Name,
            UserId = player.UserId,
            X = player.X,
            Y = player.Y,
            Z = player.Z,
            ActionPoints = player.ActionPoints,
            HealthPoints = player.HealthPoints,
            CurrentGameRoomId = player.CurrentGameRoomId,
            Profession = player.Profession,

            // mapped in Context
            Items = items,
            Skills = skillsOwned,
        };

        return playerDTO;
    }

    public List<Skills> GetOwnedSkills(Guid ownerId)
    {
        return _playerContext.Skills
            .Where(s => s.OwnerId == ownerId)
            .Select(s => s.SkillType).ToList();
    }

    public async Task<List<Item>> GetOwnedItems(Guid ownerId)
    {
        List<Item> items = await _playerContext.Items.Where(x => x.OwnerId == ownerId).ToListAsync();
        return items;
    }

    public async Task<List<Log>> GetAccessibleLogsForPlayer(Guid playerId, Guid gameId)
    {
        List<Log> publicLogsInGame = await _playerContext.Logs.Where(x => x.GameId == gameId && x.IsPublic).ToListAsync();

        List<LogAccessPermissions> permissionsForPlayer = await _playerContext.LogAccessPermission.Where(x => x.PlayerId == playerId).ToListAsync();
        List<Log> accessibleLogs = await _playerContext.LogAccessPermission.Join(_playerContext.Logs,
            accessPermission => accessPermission.LogId,
            log => log.Id,
            (a, l) => l).ToListAsync();

        List<Log> allVisibleLogs = publicLogsInGame.Union(accessibleLogs).ToList();

        return allVisibleLogs;
    }

    public async Task<List<Guid>> GetPlayersIdsWhoCanAccessLog(Log log)
    {
        List<Guid> playerIdsWhoCanSeeLogs = _playerContext.LogAccessPermission
            .Where(x => x.LogId == log.Id)
            .Select(x => x.PlayerId).ToList();

        return playerIdsWhoCanSeeLogs;
    }

    public async Task<List<Guid>> FilterPlayersWhoHaveAccessToLog(List<Guid> playerId, Log log)
    {
        if (log.IsPublic)
        {
            return playerId;
        }

        List<Guid> playersWhoCanSeeLog = await GetPlayersIdsWhoCanAccessLog(log);
        List<Guid> playersWithAccess = playerId.Where(p => HasAccessToLog(playersWhoCanSeeLog, p)).ToList();
        return playersWithAccess;
    }

    public bool HasAccessToLog(List<Guid> playersWhoHaveAccessToLog, Guid playerId)
    {
        bool hasAccess = playersWhoHaveAccessToLog.Contains(playerId);
        return hasAccess;
    }
}
