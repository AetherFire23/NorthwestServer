﻿using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Models;
namespace WebAPI.Repositories;

public class RoomRepository
{
    private readonly PlayerContext _playerContext;
    public RoomRepository(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    public async Task<List<Room>> GetAllLandmassRoomsInGame(Guid gameId)
    {
        List<Room> rooms = await _playerContext.Rooms.Where(r => r.IsLandmass && r.GameId == gameId).ToListAsync();
        return rooms;
    }

    public async Task<List<Room>> GetAllActiveLandmassRooms(Guid gameId)
    {
        List<Room> rooms = await _playerContext.Rooms.Where(r => r.IsActive && r.IsLandmass && r.GameId == gameId).ToListAsync();
        return rooms;
    }

    public async Task<Room> GetRoomById(Guid roomId)
    {
        Room room = await _playerContext.Rooms.FirstAsync(r => r.Id == roomId);
        return room;
    }

    public async Task RemoveFromAllConnectedRooms(Guid roomId)
    {
        List<AdjacentRoom> connections = await _playerContext.AdjacentRooms.Where(x => x.RoomId == roomId || x.AdjacentId == roomId).ToListAsync();
        _playerContext.RemoveRange(connections);
        _ = await _playerContext.SaveChangesAsync();
    }

    public async Task<List<Room>> GetRoomsInGamesync(Guid gameId)
    {
        List<Room> rooms = await _playerContext.Rooms.Where(x => x.GameId == gameId).ToListAsync();
        return rooms;
    }

    public async Task<Room> GetRoomFromName(Guid gameId, string roomName)
    {
        Room room = await _playerContext.Rooms.FirstAsync(r => r.GameId == gameId && r.Name == roomName);
        return room;
    }

    public async Task<RoomDto> GetRoomDTOAsync(Guid roomId)
    {
        Room requestedRoom = await GetRoomById(roomId);

        List<Player> playersInRoom = await _playerContext.Players.Where(player => player.CurrentGameRoomId == roomId).ToListAsync();

        List<Item> items = await GetRoomItems(roomId);
        List<Station> stations = await _playerContext.Stations.Where(x => x.RoomName == requestedRoom.Name).ToListAsync();

        RoomDto roomDTO = new RoomDto()
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

        return roomDTO;
    }

    public async Task<List<Item>> GetRoomItems(Guid roomId)
    {
        List<Item> items = await _playerContext.Items.Where(x => x.OwnerId == roomId).ToListAsync();
        return items;
    }

    public async Task<List<Item>> GetItemsInAllRooms(Guid gameId)
    {
        List<Room> rooms = await _playerContext.Rooms.Where(x => x.GameId == gameId).ToListAsync();
        List<Item> allItems = new List<Item>();

        foreach (Room? room in rooms)
        {
            List<Item> roomItems = await _playerContext.Items.Where(x => x.OwnerId == room.Id).ToListAsync();
            allItems.AddRange(roomItems);
        }
        return allItems;
    }

    /// <summary>
    /// Initializes level with the given gameId
    /// </summary>
    public async Task CreateNewRoomsAndConnections(Guid gameId)
    {
        Tuple<List<Room>, List<AdjacentRoom>> roomsAndConnections = DefaultRoomFactory.CreateAndInitializeNewRoomsAndConnections(gameId);

        await _playerContext.Rooms.AddRangeAsync(roomsAndConnections.Item1);
        await _playerContext.AdjacentRooms.AddRangeAsync(roomsAndConnections.Item2);
        _ = await _playerContext.SaveChangesAsync();
    }

    public async Task<List<AdjacentRoom>> GetLandmassAdjacentRoomsAsync(Guid gameId)
    {
        List<AdjacentRoom> adjacentRooms = await _playerContext.AdjacentRooms.Where(x => x.GameId == gameId && x.IsLandmassConnection).ToListAsync();
        return adjacentRooms;
    }
}
