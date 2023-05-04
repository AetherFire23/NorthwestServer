using Shared_Resources.Models;

namespace WebAPI.Interfaces
{
    public interface IChatService
    {
        public Task<ClientCallResult> CreateChatroomAsync(string playerGuid, string newRoomGuid);
        public Task<ClientCallResult> LeaveChatRoomAsync(Guid playerId, Guid roomToLeave);
        public Task<ClientCallResult> InviteToRoomAsync(Guid fromId, Guid targetPlayer, Guid targetRoomId);
        public Task PutNewMessageToServerAsync(string guid, string roomId, string receivedMessage);
        public Task<ClientCallResult> SendInviteResponse(Guid triggerId, bool isAccepted);
    }
}
