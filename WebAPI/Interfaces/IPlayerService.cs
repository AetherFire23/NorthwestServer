using Shared_Resources.Models;

namespace WebAPI.Interfaces;

public interface IPlayerService
{
    public Task<ClientCallResult> ChangeRoomAsync(Guid playerId, string targetRoomName);
    Task UpdatePositionAsync(Guid playerId, float x, float y);
    Task TransferItem(Guid targetId, Guid ownerId, Guid itemId, Guid gameId);
}
