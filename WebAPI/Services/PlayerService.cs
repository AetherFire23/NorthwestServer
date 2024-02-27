using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebAPI.Entities;
using WebAPI.Enums;
using WebAPI.Game_Actions;

namespace WebAPI.Services;

public class PlayerService
{
    private readonly PlayerContext _playerContext;
    private readonly PlayerRepository _playerRepository;
    private readonly GameActionsRepository _gameActionsRepository;
    public PlayerService(
        PlayerContext playerContext,
        PlayerRepository playerRepository,
        GameActionsRepository gameActionsRepository)
    {
        _playerContext = playerContext;
        _playerRepository = playerRepository;
        _gameActionsRepository = gameActionsRepository;
    }

    public async Task TransferItem(Guid targetId, Guid ownerId, Guid itemId, Guid gameId)
    {
        Item item = await _playerRepository.GetItemAsync(itemId);

        if (item.OwnerId != ownerId)
        {
            Console.WriteLine($"Item ownership transfer was cancelled. With Id : {item.Id} {item.ItemType} ");
        }// Ca veut dire que yer ailleurs live

        item.OwnerId = targetId;
        _ = await _playerContext.SaveChangesAsync();
    }

    public async Task UpdatePositionAsync(Guid playerId, float x, float y)
    {
        Player player = await _playerRepository.GetPlayerAsync(playerId);
        player.X = x;
        player.Y = y;
        _ = await _playerContext.SaveChangesAsync();
    }

    public async Task ChangeRoomAsync(Guid playerId, string targetRoomName)
    {
        Player player = await _playerRepository.GetPlayerAsync(playerId);
        Room currentRoom = await _playerContext.Rooms.FirstAsync(x => x.Id == player.CurrentGameRoomId);
        Room targetRoom = await _playerContext.Rooms.FirstAsync(x => x.Name == targetRoomName); // devrait pas etre avec le gameid aussi?
        bool connectionExists = await _playerContext.AdjacentRooms.FirstOrDefaultAsync(x => x.RoomId == currentRoom.Id && x.AdjacentId == targetRoom.Id) is not null;

        if (connectionExists) // should benefit from inversion 
        {
            player.CurrentGameRoomId = targetRoom.Id;
            _ = await _playerContext.SaveChangesAsync();

            Log log = new Log()
            {
                Id = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                CreatedBy = player.Name,
                EventText = $"Player {player.Name}, who was in room {currentRoom.Name}, went into {targetRoom.Name}",
                IsPublic = true,
                RoomId = targetRoom.Id,
                TriggeringPlayerId = player.Id,
                GameId = player.GameId,
            };

            GameAction gameAction = new GameAction()
            {
                Id = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                CreatedBy = player.Name,
                GameId = player.GameId,
                GameActionType = GameActionType.RoomChanged,
                SerializedProperties = JsonConvert.SerializeObject(log),
            };

            _ = await _playerContext.AddAsync(gameAction);
            _ = await _playerContext.AddAsync(log);
            _ = await _playerContext.SaveChangesAsync();
        }
        // else fail
        string message = $"Connection between {currentRoom.Name} and {targetRoom.Name} did not exist. If room is intended to be destroyed, use .IsBlockedConnection";
    }
}
