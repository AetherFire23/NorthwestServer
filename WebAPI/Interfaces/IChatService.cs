using Shared_Resources.Models;

namespace WebAPI.Interfaces;

public interface IChatService
{
    public Task<ClientCallResult> CreateChatroomAsync(string playerGuid, string newRoomGuid);
    public Task<ClientCallResult> LeaveChatRoomAsync(Guid playerId, Guid roomToLeave);
    public Task PutNewMessageToServerAsync(string guid, string roomId, string receivedMessage);
}
