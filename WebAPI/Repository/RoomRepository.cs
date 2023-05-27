using Microsoft.EntityFrameworkCore;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Models;
using System.Reflection.Metadata.Ecma335;
using WebAPI.Interfaces;

namespace WebAPI.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly PlayerContext _playerContext;
        public RoomRepository(PlayerContext playerContext)
        {
            _playerContext = playerContext;
        }

        public async Task<List<Room>> GetAllLandmassRoomsInGame(Guid gameId)
        {
            var rooms = await _playerContext.Rooms.Where(r => r.IsLandmass && r.GameId == gameId).ToListAsync();
            return rooms;
        }

        public async Task<List<Room>> GetAllActiveLandmassRooms(Guid gameId)
        {
            var rooms = await _playerContext.Rooms.Where(r => r.IsActive && r.IsLandmass && r.GameId == gameId).ToListAsync();
            return rooms;
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
            await _playerContext.SaveChangesAsync();
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

        public async Task<RoomDTO> GetRoomDTOAsync(Guid roomId)
        {
            var requestedRoom = await GetRoomById(roomId);

            var playersInRoom = await _playerContext.Players.Where(player => player.CurrentGameRoomId == roomId).ToListAsync();

            var items = await _playerContext.Items.Where(item => item.OwnerId == roomId).ToListAsync();
            var stations = await _playerContext.Stations.Where(x => x.RoomName == requestedRoom.Name).ToListAsync();

            RoomDTO roomDTO = new RoomDTO()
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

        /// <summary>
        /// Initializes level with the given gameId
        /// </summary>
        public async Task CreateNewRoomsAndConnections(Guid gameId)
        {
            Tuple<List<Room>, List<AdjacentRoom>> roomsAndConnections = DefaultRoomFactory.CreateAndInitializeNewRoomsAndConnections(gameId);

            await _playerContext.Rooms.AddRangeAsync(roomsAndConnections.Item1);
            await _playerContext.AdjacentRooms.AddRangeAsync(roomsAndConnections.Item2);
            await _playerContext.SaveChangesAsync();
        }

        public async Task<List<AdjacentRoom>> GetLandmassAdjacentRoomsAsync(Guid gameId)
        {
            var adjacentRooms = await _playerContext.AdjacentRooms.Where(x => x.GameId == gameId && x.IsLandmassConnection).ToListAsync();
            return adjacentRooms;
        }
    }
}
