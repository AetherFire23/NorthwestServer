using Shared_Resources.Models;

namespace WebAPI.Interfaces
{
    public interface IPlayerService
    {
        public Task TransferItem(Guid targetId, Guid itemId, Guid gameId);
        public Task<ClientCallResult> ChangeRoomAsync(Guid playerId, string targetRoomName);
        Task UpdatePositionAsync(Guid playerId, float x, float y);
    }
}
