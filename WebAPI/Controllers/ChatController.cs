using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared_Resources.Models;
using System.Security.AccessControl;
using WebAPI.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly PlayerContext _playerContext;
        private readonly IChatService _chatService;
        public ChatController(IPlayerRepository playerRepository, 
            PlayerContext playerContext, 
            IChatService chatService)
        {
            _playerRepository = playerRepository;
            _playerContext = playerContext;
            _chatService = chatService;
        }

        [HttpPut]
        [Route("CreateChatroom")] // used
        public async Task<ActionResult<ClientCallResult>> CreateChatroom(string playerGuid, string newRoomGuid)
        {
            ClientCallResult result = await _chatService.CreateChatroomAsync(playerGuid, newRoomGuid);
            return Ok(result);
        }

        [HttpPut] // put = update, post = creation
        [Route("LeaveChatRoom")] // used 
        public async Task<ActionResult<ClientCallResult>> LeaveChatRoom(Guid playerId, Guid roomToLeave) // va dependre de comment je manage les data
        {
            ClientCallResult result = await _chatService.LeaveChatRoomAsync(playerId, roomToLeave);
            return Ok(result);
        }

        [HttpPut]
        [Route("InviteToRoom")]
        public async Task<ActionResult<ClientCallResult>> InviteToRoom(Guid fromId, Guid targetPlayer, Guid targetRoomId) 
        {
            await _chatService.InviteToRoomAsync(fromId, targetPlayer, targetRoomId);
            // demander a ben pour quand jai besoin de props + la base class
            return Ok(ClientCallResult.Success);
        }

        [HttpPut]
        [Route("PutNewMessageToServer")]
        public async Task<ActionResult> PutNewMessageToServer(string guid, string roomId, string receivedMessage)
        {
            await _chatService.PutNewMessageToServerAsync(guid, roomId, receivedMessage);
            return Ok();
        }

        [HttpPut]
        [Route("SendInviteResponse")]
        public async Task<ActionResult<ClientCallResult>> SendInviteResponse(Guid triggerId, bool isAccepted)
        {
            await _chatService.SendInviteResponse(triggerId, isAccepted);
            return ClientCallResult.Success;
        }
    }
}
