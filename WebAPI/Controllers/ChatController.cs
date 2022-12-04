using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.AccessControl;
using WebAPI.Db_Models;
using WebAPI.Enums;
using WebAPI.Interfaces;
using WebAPI.Models;

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
            ClientCallResult result = _chatService.CreateChatroom(playerGuid, newRoomGuid);
            return Ok(result);
        }

        [HttpPut] // put = update, post = creation
        [Route("LeaveChatRoom")] // used 
        public async Task<ActionResult<ClientCallResult>> LeaveChatRoom(Guid playerId, Guid roomToLeave) // va dependre de comment je manage les data
        {
            ClientCallResult result = _chatService.LeaveChatRoom(playerId, roomToLeave);
            return Ok(result);
        }

        [HttpPut]
        [Route("InviteToRoom")]
        public async Task<ActionResult<ClientCallResult>> InviteToRoom(Guid fromId, Guid targetPlayer, Guid targetRoomId) 
        {
            _chatService.InviteToRoom(fromId, targetPlayer, targetRoomId);
            // demander a ben pour quand jai besoin de props + la base class
            return Ok(ClientCallResult.Success);
        }

        [HttpPut]
        [Route("PutNewMessageToServer")]
        public async Task<ActionResult> PutNewMessageToServer(string guid, string roomId, string receivedMessage)
        {
            _chatService.PutNewMessageToServer(guid, roomId, receivedMessage);
            return Ok();
        }

        [HttpPut]
        [Route("SendInviteResponse")]
        public async Task<ActionResult<ClientCallResult>> SendInviteResponse(Guid triggerId, bool isAccepted)
        {
            _chatService.SendInviteResponse(triggerId, isAccepted);
            return ClientCallResult.Success;
        }
    }
}
