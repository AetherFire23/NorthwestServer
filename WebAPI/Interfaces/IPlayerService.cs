using Shared_Resources.Entities;
using Shared_Resources.Models;

namespace WebAPI.Interfaces
{
    public interface IPlayerService
    {
        public Task TransferItem(Guid targetId, Guid itemId);
        public Task UpdatePositionAsync(Player unityPlayerModel);
        public Task<ClientCallResult> ChangeRoomAsync(Guid playerId, string targetRoomName);
    }
}
