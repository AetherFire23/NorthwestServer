

using Shared_Resources.Models;

namespace WebAPI.Interfaces
{
    public interface IChatService
    {
        public ClientCallResult CreateChatroom(string playerGuid, string newRoomGuid);
        public ClientCallResult LeaveChatRoom(Guid playerId, Guid roomToLeave);
        public ClientCallResult InviteToRoom(Guid fromId, Guid targetPlayer, Guid targetRoomId);
        public void PutNewMessageToServer(string guid, string roomId, string receivedMessage);
        public ClientCallResult SendInviteResponse(Guid triggerId, bool isAccepted);
    }
}
