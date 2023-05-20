using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz.Core;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using System.Text;
using WebAPI;
using System;
using System.Collections.Generic;
public class PlayerRepository : IPlayerRepository
{
    PlayerContext _playerContext;

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
        var playerInvites = _playerContext.Invitations.Where(invite => invite.ToPlayerId == playerId).ToList();
        return playerInvites;
    }

    public List<Player> GetPlayersInGame(Guid gameId)
    {
        return Players.Where(player => player.GameId == gameId).ToList();
    }

    public async Task<PlayerDTO> MapPlayerDTOAsync(Guid playerId)
    {
        Player player = await GetPlayerAsync(playerId);

        List<Item> items = GetOwnedItems(playerId).ToList();

        List<SkillType> skillsOwned = GetOwnedSkills(playerId);

        PlayerDTO playerDTO = new PlayerDTO()
        {
            // Common with player
            Id = playerId,
            GameId = player.GameId,
            Name = player.Name,
            X = player.X,
            Y = player.Y,
            Z = player.Z,
            ActionPoints = player.ActionPoints,
            HealthPoints = player.HealthPoints,
            CurrentChatRoomId = player.CurrentChatRoomId,
            CurrentGameRoomId = player.CurrentGameRoomId,
            Profession = player.Profession,

            // mapped in Context
            Items = items,
            Skills = skillsOwned,
        };

        return playerDTO;
    }

    public List<SkillType> GetOwnedSkills(Guid ownerId)
    {
        return _playerContext.Skills.Where(s => s.OwnerId == ownerId)
            .Select(s => s.SkillType).ToList();
    }

    public List<Item> GetOwnedItems(Guid ownerId)
    {
        return _playerContext.Items.Where(item => item.OwnerId == ownerId).ToList();
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

    public async Task<List<Log>> GetAccessibleLogs(Guid playerId, Guid gameId,DateTime? lastTimeStamp)
    { // ne prend pas en compte le gameId haha
        var logs = new List<Log>();

        var newServerLogs = lastTimeStamp is null
            ? await _playerContext.Logs.Where(x => x.GameId == gameId).ToListAsync()
            : await _playerContext.Logs.Where(x => x.GameId == gameId && x.Created > lastTimeStamp).ToListAsync();

        foreach (var log in newServerLogs)
        {
            if (log.IsPublic)
            {
                logs.Add(log);
            }

            else
            {
                // should encapsulate like RoomLogIsAccessibleTo(LogId, playerId)
                var playersIdsWhoCanSeeLog = _playerContext.LogAccessPermission
                    .Where(x => x.LogId == log.Id)
                    .Select(x => x.PlayerId);

                bool canSeePrivateLog = playersIdsWhoCanSeeLog.Contains(playerId);

                if (canSeePrivateLog)
                {
                    logs.Add(log);
                }
            }
        }

        return logs;
    }
}
