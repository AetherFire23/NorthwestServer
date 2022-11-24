using WebAPI.Db_Models;

namespace WebAPI.Interfaces
{
    public interface IGameActionsRepository
    {
        public void ChangeRoomAction(Player p, Room from, Room to);
    }
}
