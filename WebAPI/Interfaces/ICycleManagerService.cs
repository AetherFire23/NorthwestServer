using WebAPI.Entities;

namespace WebAPI.Interfaces
{
    public interface ICycleManagerService
    {
        public void TickGame(Guid GameId);
    }
}
