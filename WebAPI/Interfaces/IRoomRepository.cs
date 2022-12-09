using WebAPI.DTOs;
using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IRoomRepository
    {
        public void CreateNewRooms(Guid gameId);

        public LevelTemplate BuildLevelTemplate();

        public RoomDTO GetRoomDTO(Guid roomId);
    }
}