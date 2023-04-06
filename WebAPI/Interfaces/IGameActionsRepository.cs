using Shared_Resources.Entities;

namespace WebAPI.Interfaces
{
    public interface IGameActionsRepository
    {
        public void ChangeRoomAction(Player p, Room from, Room to);
    }
}
