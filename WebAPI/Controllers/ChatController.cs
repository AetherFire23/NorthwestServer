using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.AccessControl;
using WebAPI.Db_Models;
using WebAPI.Enums;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly PlayerContext _playerContext;
        public ChatController(IPlayerRepository playerRepository, PlayerContext playerContext)
        {
            _playerRepository = playerRepository;
            _playerContext = playerContext;
        }

        [HttpPut]
        [Route("CreateChatroom")] // used
        public async Task<ActionResult<ClientCallResult>> CreateChatroom(string playerGuid, string newRoomGuid)
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

            return Ok();
        }

        [HttpPut] // put = update, post = creation
        [Route("LeaveChatRoom")] // used 
        public async Task<ActionResult<ClientCallResult>> LeaveChatRoom(Guid playerId, Guid roomToLeave) // va dependre de comment je manage les data
        {
            var t = _playerContext.PrivateChatRooms.First(x => x.RoomId == roomToLeave && x.ParticipantId == playerId);
            _playerContext.PrivateChatRooms.Remove(t);
            _playerContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("InviteToRoom")]
        public async Task<ActionResult<ClientCallResult>> InviteToRoom(Guid fromId, Guid targetPlayer) // va dependre de comment je manage les data
        {
            var invitation = new PrivateInvitationNotification()
            {
                FromPlayerId = fromId,
                IsAccepted = false,
                FromPlayerName = _playerRepository.GetPlayer(fromId).Name,
                RoomId = Guid.NewGuid(),
                ToPlayerName = _playerRepository.GetPlayer(targetPlayer).Name,
            };

            var newNotif = new TriggerNotification()
            {
                Id = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                GameActionId = Guid.Empty,
                IsReceived = false,
                NotificationType = NotificationType.PrivInv,
                PlayerId = targetPlayer,
                SerializedProperties = JsonConvert.SerializeObject(invitation)
            };

            _playerContext.TriggerNotifications.Add(newNotif);

            _playerContext.SaveChanges();
            return Ok(ClientCallResult.Success);
        }

        [HttpPut]
        [Route("PutNewMessageToServer")]
        public async Task<ActionResult> PutNewMessageToServer(string guid, string roomId, string receivedMessage)
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
            return Ok();
        }

        [HttpPut]
        [Route("SendInviteResponse")]
        public async Task<ActionResult<ClientCallResult>> SendInviteResponse(Guid triggerId, bool isAccepted)
        {
            var invite = _playerContext.TriggerNotifications.First(x => x.Id == triggerId);

            var props = JsonConvert.DeserializeObject<PrivateInvitationNotification>(invite.SerializedProperties);
            var newPair = new PrivateChatRoomParticipant()
            {
                Id = Guid.NewGuid(),
                ParticipantId = invite.PlayerId,
                RoomId = props.RoomId,
            };

            _playerContext.SaveChanges();

            return ClientCallResult.Success;
        }
    }
}
