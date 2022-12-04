using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using WebAPI;
using WebAPI.Db_Models;
using WebAPI.DTOs;
using WebAPI.Enums;

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

    public Player GetPlayer(Guid id)
    {
        return _playerContext.Players.First(queriedPlayer => queriedPlayer.Id == id);
    }

    public Item GetItem(Guid itemId)
    {
        return _playerContext.Items.First(x=> x.Id == itemId);
    }

    public List<PrivateInvitation> GetPlayerInvitations(Guid playerId)
    {
        var playerInvites = _playerContext.Invitations.Where(invite => invite.ToPlayerId == playerId).ToList();
        return playerInvites;
    }

    public List<Player> GetPlayersInGame(Guid gameId)
    {
        return Players.Where(player => player.GameId == gameId).ToList();
    }

    public PlayerDTO MapPlayerDTO(Guid playerId)
    {
        Player player = GetPlayer(playerId);

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

    public List<TriggerNotificationDTO> GetTriggerNotifications(Guid playerId, DateTime? timeStamp)
    {
        List<TriggerNotification> notifications = new();
        if(timeStamp is null)
        {
            notifications = _playerContext.TriggerNotifications.Where(x=> x.PlayerId == playerId && !x.IsReceived).ToList();
        }

        else
        {
            notifications = _playerContext.TriggerNotifications.Where(x => x.PlayerId == playerId && x.Created > timeStamp && !x.IsReceived).ToList();
        }

        foreach (var notification in notifications)
        {
            notification.IsReceived = true;
            _playerContext.TriggerNotifications.Update(notification); 
        }

        _playerContext.SaveChanges();

        return notifications.Select(x => x.ToDTO()).ToList();
    }

    public List<TriggerNotification> GetAllTriggersOfType(Guid playerId, NotificationType notificationType)
    {
        var triggersOfType = _playerContext.TriggerNotifications.Where(x=> x.Id == playerId && x.NotificationType == notificationType).ToList();
        return triggersOfType;
    }
}
