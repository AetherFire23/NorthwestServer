using WebAPI.Db_Models;
using WebAPI.DTOs;
using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IRoomRepository
    {
        /// <summary>
        /// Initializes empty rooms with given gameId and adds it to the database.
        /// </summary>
        /// <param name="gameId"></param>
        public void CreateNewRooms(Guid gameId);
        public List<Room> GetRoomsInGame(Guid gameId);
        public RoomDTO GetRoomDTO(Guid roomId);
        public Room GetRoomFromName(Guid gameId, string roomName);
        public void RemoveFromAllConnectedRooms(Guid roomId);
        public List<Room> GetAllLandmassRooms(Guid gameId);
        public List<Room> GetAllActiveLandmassRooms(Guid gameId);
    }
}