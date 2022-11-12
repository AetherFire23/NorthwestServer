using WebAPI.Models;

namespace WebAPI.Room_template
{
    public interface IRoomRepository
    {

        public void CreateNewGameRooms();
        public LevelTemplate BuildLevelTemplate();
    }
}