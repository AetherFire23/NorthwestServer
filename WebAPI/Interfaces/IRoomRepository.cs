using Shared_Resources.DTOs;
using Shared_Resources.Entities;

namespace WebAPI.Interfaces
{
    public interface IRoomRepository
    {
        /// <summary>
        /// Initializes empty rooms with given gameId and adds it to the database.
        /// </summary>
        /// <param name="gameId"></param>
        Task<List<Room>> GetAllLandmassRoomsInGame(Guid gameId);
        Task<List<Room>> GetAllActiveLandmassRooms(Guid gameId);
        Task<Room> GetRoomById(Guid roomId);
        Task RemoveFromAllConnectedRooms(Guid roomId);
        Task<List<Room>> GetRoomsInGame(Guid gameId);
        Task<Room> GetRoomFromName(Guid gameId, string roomName);
        Task<RoomDTO> GetRoomDTOAsync(Guid roomId);
        Task CreateNewRooms(Guid gameId);
        Task<List<AdjacentRoom>> GetLandmassAdjacentRoomsAsync(Guid gameId);
    }
}