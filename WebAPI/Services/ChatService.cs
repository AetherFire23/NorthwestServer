using Microsoft.EntityFrameworkCore;
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
        public async Task<ClientCallResult> CreateChatroomAsync(string playerGuid, string newRoomGuid)
        {
            Guid playerid = new Guid(playerGuid);
            Guid newRoomid = new Guid(newRoomGuid);

            var pair = new PrivateChatRoomParticipant()
            {
                Id = Guid.NewGuid(),
                ParticipantId = playerid,
                RoomId = newRoomid
            };

            _playerContext.PrivateChatRoomParticipants.Add(pair);
            await _playerContext.SaveChangesAsync();

            return ClientCallResult.Success;
        }

        public async Task<ClientCallResult> LeaveChatRoomAsync(Guid playerId, Guid roomToLeave)
        {
            var t = await _playerContext.PrivateChatRoomParticipants.FirstAsync(x => x.RoomId == roomToLeave && x.ParticipantId == playerId);
            _playerContext.PrivateChatRoomParticipants.Remove(t);
            await _playerContext.SaveChangesAsync();
            return ClientCallResult.Success;
        }

        public async Task<ClientCallResult> InviteToRoomAsync(Guid fromId, Guid targetPlayer, Guid targetRoomId)
        {
            var invites = _playerRepository.GetAllTriggersOfType(targetPlayer, NotificationType.PrivInv);

            foreach (var invite in invites)
            {
                var props = JsonConvert.DeserializeObject<PrivateInvitationProperties>(invite.SerializedProperties);
                bool inviteExists = props.FromPlayerId == fromId
                                    && invite.PlayerId == targetPlayer
                                    && props.RoomId == targetRoomId;

                if (!inviteExists) continue;
                var result = new ClientCallResult()
                {
                    IsSuccessful = false,
                    Message = "Invite already exists"
                };

                return result;
            }

            var fromPlayer = await _playerRepository.GetPlayerAsync(fromId);
            var targetPlayerEnt = await _playerRepository.GetPlayerAsync(targetPlayer);

            var invitationProperties = new PrivateInvitationProperties()
            {
                FromPlayerId = fromId,
                IsAccepted = false,
                FromPlayerName = fromPlayer.Name,
                RoomId = targetRoomId,  // ca prend la roomId 
                ToPlayerName = targetPlayerEnt.Name,
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

            await _playerContext.SaveChangesAsync();

            return ClientCallResult.Success;
        }

        public async Task PutNewMessageToServerAsync(string guid, string roomId, string receivedMessage)
        {
            var player = await _playerRepository.GetPlayerAsync(new Guid(guid));

            var message = new Message()
            {
                Id = Guid.NewGuid(),
                GameId = player.GameId,
                Text = receivedMessage,
                SenderName = player.Name,
                RoomId = new Guid(roomId),
                Created = DateTime.UtcNow,
            };

            _playerContext.Messages.Add(message);
            await _playerContext.SaveChangesAsync();
        }

        public async Task<ClientCallResult> SendInviteResponse(Guid triggerId, bool isAccepted)
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

            _playerContext.PrivateChatRoomParticipants.Add(newPair);

            await _playerContext.SaveChangesAsync();
            return ClientCallResult.Success;
        }
    }
}
