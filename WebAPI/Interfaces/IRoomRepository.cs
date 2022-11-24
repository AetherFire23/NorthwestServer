using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IRoomRepository
    {

        public void CreateNewGameRooms();
        public LevelTemplate BuildLevelTemplate();
    }
}