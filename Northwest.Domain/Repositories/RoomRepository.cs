using Microsoft.EntityFrameworkCore;
using Northwest.Domain.Dtos;
using Northwest.Domain.Models;
using Northwest.Persistence;
using Northwest.Persistence.Entities;

namespace Northwest.Domain.Repositories;

public class RoomRepository
{
    private readonly PlayerContext _playerContext;
    public RoomRepository(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    public async Task<Room> GetRoomById(Guid roomId)
    {
        var room = await _playerContext.Rooms.FirstAsync(r => r.Id == roomId);
        return room;
    }

    public async Task RemoveFromAllConnectedRooms(Guid roomId)
    {
        var connections = await _playerContext.AdjacentRooms.Where(x => x.RoomId == roomId || x.AdjacentId == roomId).ToListAsync();
        _playerContext.RemoveRange(connections);
        _ = await _playerContext.SaveChangesAsync();
    }

    public async Task<List<Room>> GetRoomsInGamesync(Guid gameId)
    {
        var rooms = await _playerContext.Rooms.Where(x => x.GameId == gameId).ToListAsync();
        return rooms;
    }

    public async Task<Room> GetRoomFromName(Guid gameId, string roomName)
    {
        var room = await _playerContext.Rooms.FirstAsync(r => r.GameId == gameId && r.Name == roomName);
        return room;
    }

    public async Task<RoomDto> GetRoomDtoAsync(Guid roomId)
    {
        var requestedRoom = await GetRoomById(roomId);
        var playersInRoom = await _playerContext.Players.Where(player => player.CurrentGameRoomId == roomId).ToListAsync();
        var items = await GetRoomItems(roomId);
        var stations = await _playerContext.Stations.Where(x => x.RoomName == requestedRoom.Name).ToListAsync();

        return new()
        {
            Id = requestedRoom.Id,
            Items = items,
            Players = playersInRoom,
            Name = requestedRoom.Name,
            GameId = requestedRoom.GameId,
            Stations = stations,
            IsLandmass = requestedRoom.IsLandmass,
            X = requestedRoom.X,
            Y = requestedRoom.Y,
        };
    }

    public async Task<List<Item>> GetRoomItems(Guid roomId)
    {
        var items = await _playerContext.Items.Where(x => x.OwnerId == roomId).ToListAsync();
        return items;
    }

    public async Task<List<Item>> GetItemsInAllRooms(Guid gameId)
    {
        var rooms = await _playerContext.Rooms.Where(x => x.GameId == gameId).ToListAsync();
        var allItems = new List<Item>();

        foreach (var room in rooms)
        {
            var roomItems = await _playerContext.Items.Where(x => x.OwnerId == room.Id).ToListAsync();
            allItems.AddRange(roomItems);
        }
        return allItems;
    }

    /// <summary>
    /// Initializes level with the given gameId
    /// </summary>
    public async Task CreateNewRoomsAndConnections(Guid gameId)
    {
        var roomsAndConnections = DefaultRoomFactory.CreateAndInitializeNewRoomsAndConnections(gameId);

        await _playerContext.Rooms.AddRangeAsync(roomsAndConnections.Item1);
        await _playerContext.AdjacentRooms.AddRangeAsync(roomsAndConnections.Item2);
        await _playerContext.SaveChangesAsync();
    }
}
