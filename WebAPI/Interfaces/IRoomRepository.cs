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
        public List<Room> GetAllRooms(Guid gameId);
        public RoomDTO GetRoomDTO(Guid roomId);
    }
}