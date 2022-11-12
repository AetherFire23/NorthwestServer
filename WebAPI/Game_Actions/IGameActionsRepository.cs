using WebAPI.Models;

namespace WebAPI.Game_Actions
{
    public interface IGameActionsRepository
    {
        public void ChangeRoomAction(Player p, Room from, Room to);
    }
}
