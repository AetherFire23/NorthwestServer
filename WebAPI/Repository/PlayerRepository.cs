using Microsoft.EntityFrameworkCore;
using System.Text;
using WebAPI;
using WebAPI.Enums;
using WebAPI.Models;
using WebAPI.Models.DTOs;

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
        return _playerContext.Players.FirstOrDefault(queriedPlayer => queriedPlayer.Id == id);
    }

    public List<PrivateInvitation> GetPlayerInvitations(Guid playerId)
    {
        var playerInvites = _playerContext.Invitations.Where(invite => invite.ToPlayerId == playerId).ToList();
        return playerInvites;
    }

    public List<Player> GetPlayersInCurrentGame(Guid gameId)
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
        return this._playerContext.Items.Where(item => item.OwnerId == ownerId).ToList();
    }

    public RoomDTO GetRoomDTO(Guid roomId)
    {
        Room requestedRoom = _playerContext.Rooms.FirstOrDefault(room => room.Id == roomId);

        if(requestedRoom is null)
        {
            return new RoomDTO();
        }

        var playersInRoom = _playerContext.Players.Where(player => player.CurrentGameRoomId == roomId).ToList();

        var items = _playerContext.Items.Where(item => item.OwnerId == roomId).ToList();

        RoomDTO roomDTO = new RoomDTO()
        {
            Id = requestedRoom.Id,
            RoomType = requestedRoom.RoomType,
            Items = items,
            Players = playersInRoom,
        };

        return roomDTO;
    }
}