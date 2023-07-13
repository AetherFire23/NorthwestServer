using Microsoft.EntityFrameworkCore;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Interfaces;
using Shared_Resources.Models;
using System.Runtime.CompilerServices;
using WebAPI;
public class PlayerRepository : IPlayerRepository
{
    private readonly PlayerContext _playerContext;

    DbSet<Player> Players => _playerContext.Players;

    public PlayerRepository(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    public Player GetPlayer(string name)
    {
        Player? possiblePlayer = _playerContext.Players.FirstOrDefault(player => player.Name.Equals(name));

        if (possiblePlayer is null)
        {
            throw new NotImplementedException("PlayerNotFound");
        }

        return possiblePlayer;
    }

    public async Task<Player> GetPlayerAsync(Guid id)
    {
        var player = await _playerContext.Players.FirstAsync(queriedPlayer => queriedPlayer.Id == id);
        return player;
    }

    public async Task<Item> GetItemAsync(Guid itemId)
    {
        var item = await _playerContext.Items.FirstAsync(x => x.Id == itemId);
        return item;
    }

    public async Task<List<PrivateInvitation>> GetPlayerInvitations(Guid playerId)
    {
        var playerInvites = await _playerContext.Invitations.Where(invite => invite.ToPlayerId == playerId).ToListAsync();
        return playerInvites;
    }

    public async Task<List<Player>> GetPlayersInGameAsync(Guid gameId)
    {
        var allPlayers = await Players.Where(player => player.GameId == gameId).ToListAsync();
        return allPlayers;
    }

    public async Task<PlayerDTO> MapPlayerDTOAsync(Guid playerId)
    {
        Player player = await GetPlayerAsync(playerId);

        List<Item> items = await GetOwnedItems(playerId);

        List<SkillEnum> skillsOwned = GetOwnedSkills(playerId);

        PlayerDTO playerDTO = new PlayerDTO()
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

    public List<SkillEnum> GetOwnedSkills(Guid ownerId)
    {
        return _playerContext.Skills
            .Where(s => s.OwnerId == ownerId)
            .Select(s => s.SkillType).ToList();
    }

    public async Task<List<Item>> GetOwnedItems(Guid ownerId)
    {
        var items = await _playerContext.Items.Where(x => x.OwnerId == ownerId).ToListAsync();
        return items;
    }

    public async Task<List<TriggerNotificationDTO>> GetTriggerNotificationsAsync(Guid playerId, DateTime? timeStamp)
    {
        List<TriggerNotification> notifications = new();

        if (timeStamp is null)
        {
            notifications = await _playerContext.TriggerNotifications.Where(x => x.PlayerId == playerId && !x.IsReceived).ToListAsync();
        }

        else
        {
            notifications = await _playerContext.TriggerNotifications.Where(x => x.PlayerId == playerId && x.Created > timeStamp && !x.IsReceived).ToListAsync();
        }

        foreach (var notification in notifications)
        {
            notification.IsReceived = true;
            _playerContext.TriggerNotifications.Update(notification);
        }

        await _playerContext.SaveChangesAsync();

        return notifications.Select(x => x.ToDTO()).ToList();
    }

    public List<TriggerNotification> GetAllTriggersOfType(Guid playerId, NotificationType notificationType)
    {
        var triggersOfType = _playerContext.TriggerNotifications.Where(x => x.Id == playerId && x.NotificationType == notificationType).ToList();
        return triggersOfType;
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
        var playerIdsWhoCanSeeLogs = _playerContext.LogAccessPermission
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

        var playersWhoCanSeeLog = await GetPlayersIdsWhoCanAccessLog(log);
        var playersWithAccess = playerId.Where(p => HasAccessToLog(playersWhoCanSeeLog, p)).ToList();
        return playersWithAccess;
    }

    public bool HasAccessToLog(List<Guid> playersWhoHaveAccessToLog, Guid playerId)
    {
        bool hasAccess = playersWhoHaveAccessToLog.Contains(playerId);
        return hasAccess;
    }
}
