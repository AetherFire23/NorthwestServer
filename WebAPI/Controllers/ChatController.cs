using Microsoft.AspNetCore.Mvc;
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
        [Route("PutCreateChatRoom")] // pense obsolet 
        public async Task<ActionResult<string>> PutCreatePrivateChatRoom(string playerGuid) // UpdateOrCreate ? pour 
        {
            Guid playerId = new Guid(playerGuid);
            //var player = _playerRepository.GetPlayer(playerId);

            var privateRoomPair = new PrivateChatRoomParticipant
            {
                Id = Guid.NewGuid(),
                ParticipantId = playerId,
                RoomId = Guid.NewGuid(),
            };
            _playerContext.PrivateChatRooms.Add(privateRoomPair);
            _playerContext.SaveChanges();
            return Ok("lol");
        }

        [HttpPut]
        [Route("InviteToChatRoom")]
        public async Task<ActionResult<string>> InvitePlayerToChatRoom([FromBody] PrivateInvitation invitation)
        {
            Player requestingPlayer = _playerRepository.GetPlayer(invitation.FromPlayerId);

            //must complete the invitedusername
            invitation.ToPlayerName = _playerRepository.GetPlayer(invitation.ToPlayerId).Name;

            bool playerAlreadyInRoom = _playerContext.PrivateChatRooms.Any(x => x.RoomId == invitation.RoomId && x.ParticipantId == invitation.ToPlayerId); // should be able to replace those two horrible queries
            bool identicalInvitationExists = _playerContext.Invitations.Any(x => x.RoomId == invitation.RoomId && x.ToPlayerId == invitation.ToPlayerId);

            if (playerAlreadyInRoom || identicalInvitationExists)
            {
                return "yeah!";
            }
            _playerContext.Invitations.Add(invitation);
            _playerContext.SaveChanges();
            return Ok("yeah");
        }

        [HttpPut]
        [Route("SendInvitationResponse")]
        public async Task<string> SendPrivateChatRoomInviteResponse([FromBody] PrivateInvitation invitation)
        {
            var invite = _playerContext.Invitations.FirstOrDefault(invite => invite.Id == invitation.Id);

            invite.FromPlayerId = invitation.FromPlayerId;
            invite.FromPlayerName = invitation.FromPlayerName;
            invite.ToPlayerName = invitation.ToPlayerName;
            invite.ToPlayerId = invitation.ToPlayerId;
            invite.IsAccepted = invitation.IsAccepted;
            invite.RequestFulfilled = true;
            invite.RoomId = invitation.RoomId;

            var invitingPlayer = _playerRepository.GetPlayer(invite.FromPlayerId);

            if (invite.IsAccepted)
            {
                //specify that the player is in that room
                var privatePair = new PrivateChatRoomParticipant()
                {
                    Id = Guid.NewGuid(),
                    RoomId = invitation.RoomId,
                    ParticipantId = invite.ToPlayerId,

                };

                // I dont need the invitation after having resolved it 
                _playerContext.Invitations.Remove(invite); // Removed quand yer accepted mais pas removed quand yer refuse ? possibilite de bug 

                _playerContext.PrivateChatRooms.Add(privatePair);
            }

            _playerContext.SaveChanges();

            return $"You successfully invited {invitingPlayer.Name} to your private chat room. ";
        }
    }
}
