using WebAPI.Entities;

namespace WebAPI.Interfaces
{
    public interface ICycleManager
    {
        public void TickGame(Guid GameId);
    }
}
