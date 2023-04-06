using Newtonsoft.Json;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class ChatService : IChatService
    {
        private readonly PlayerContext _playerContext;
        private readonly IPlayerRepository _playerRepository;
        public ChatService(PlayerContext playerContext, IPlayerRepository playerRepository)
        {
            _playerContext = playerContext;
            _playerRepository = playerRepository;
        }
        public ClientCallResult CreateChatroom(string playerGuid, string newRoomGuid)
        {
            Guid playerid = new Guid(playerGuid);
            Guid newRoomid = new Guid(newRoomGuid);

            var pair = new PrivateChatRoomParticipant()
            {
                Id = Guid.NewGuid(),
                ParticipantId = playerid,
                RoomId = newRoomid
            };

            _playerContext.PrivateChatRooms.Add(pair);
            _playerContext.SaveChanges();

            return ClientCallResult.Success;
        }

        public ClientCallResult LeaveChatRoom(Guid playerId, Guid roomToLeave)
        {
            var t = _playerContext.PrivateChatRooms.First(x => x.RoomId == roomToLeave && x.ParticipantId == playerId);
            _playerContext.PrivateChatRooms.Remove(t);
            _playerContext.SaveChanges();
            return ClientCallResult.Success;
        }

        public ClientCallResult InviteToRoom(Guid fromId, Guid targetPlayer, Guid targetRoomId)
        {
            var invites = _playerRepository.GetAllTriggersOfType(targetPlayer, NotificationType.PrivInv);

            foreach (var invite in invites)
            {
                var props = JsonConvert.DeserializeObject<PrivateInvitationProperties>(invite.SerializedProperties);
                bool inviteExists = props.FromPlayerId == fromId && invite.PlayerId == targetPlayer && props.RoomId == targetRoomId;
                if (inviteExists)
                {
                    var result = new ClientCallResult()
                    {
                        IsSuccessful = false,
                        Message = "Invite already exists"
                    };

                    return result;
                }
            }

            var invitationProperties = new PrivateInvitationProperties()
            {
                FromPlayerId = fromId,
                IsAccepted = false,
                FromPlayerName = _playerRepository.GetPlayer(fromId).Name,
                RoomId = targetRoomId,  // ca prend la roomId 
                ToPlayerName = _playerRepository.GetPlayer(targetPlayer).Name,
            };

            var trigger = new TriggerNotification()
            {
                Id = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                GameActionId = Guid.Empty,
                IsReceived = false,
                NotificationType = NotificationType.PrivInv,
                PlayerId = targetPlayer,
                SerializedProperties = JsonConvert.SerializeObject(invitationProperties)
            };

            _playerContext.TriggerNotifications.Add(trigger);

            _playerContext.SaveChanges();

            return ClientCallResult.Success;
        }

        public void PutNewMessageToServer(string guid, string roomId, string receivedMessage)
        {
            var player = _playerRepository.GetPlayer(new Guid(guid));

            var message = new Message()
            {
                Id = Guid.NewGuid(),
                GameId = player.GameId,
                Text = receivedMessage,
                Name = player.Name,
                RoomId = new Guid(roomId),
                Created = DateTime.UtcNow,
            };

            _playerContext.Messages.Add(message);
            _playerContext.SaveChanges();
        }

        public ClientCallResult SendInviteResponse(Guid triggerId, bool isAccepted)
        {
            if (!isAccepted)
            {
                return ClientCallResult.Success;
            }

            var invite = _playerContext.TriggerNotifications.First(x => x.Id == triggerId);

            var props = JsonConvert.DeserializeObject<PrivateInvitationProperties>(invite.SerializedProperties);
            var newPair = new PrivateChatRoomParticipant()
            {
                Id = Guid.NewGuid(),
                ParticipantId = invite.PlayerId,
                RoomId = props.RoomId,
            };

            _playerContext.PrivateChatRooms.Add(newPair);

            _playerContext.SaveChanges();
            return ClientCallResult.Success;
        }
    }
}
