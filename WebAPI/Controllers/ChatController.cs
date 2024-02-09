using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;
namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;
    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPut]
    [Route("CreateChatroom")] // used
    public async Task<ActionResult> CreateChatroom(string playerGuid, string newRoomGuid)
    {
        await _chatService.CreateChatroomAsync(playerGuid, newRoomGuid);
        return Ok();
    }

    [HttpPut] // put = update, post = creation
    [Route("LeaveChatRoom")] // used 
    public async Task<ActionResult> LeaveChatRoom(Guid playerId, Guid roomToLeave) // va dependre de comment je manage les data
    {
        await _chatService.LeaveChatRoomAsync(playerId, roomToLeave);
        return Ok();
    }


    // need to refactor using SSE client
    //[HttpPut]
    //[Route("InviteToRoom")]
    //public async Task<ActionResult<ClientCallResult>> InviteToRoom(Guid fromId, Guid targetPlayer, Guid targetRoomId)
    //{
    //    await _chatService.InviteToRoomAsync(fromId, targetPlayer, targetRoomId);
    //    // demander a ben pour quand jai besoin de props + la base class
    //    return Ok(ClientCallResult.Success);
    //}

    [HttpPut]
    [Route("PutNewMessageToServer")]
    public async Task<ActionResult> PutNewMessageToServer(string guid, string roomId, string receivedMessage)
    {
        await _chatService.PutNewMessageToServerAsync(guid, roomId, receivedMessage);
        return Ok();
    }



    // refactor using sse client
    //[HttpPut]
    //[Route("SendInviteResponse")]
    //public async Task<ActionResult<ClientCallResult>> SendInviteResponse(Guid triggerId, bool isAccepted)
    //{
    //    await _chatService.SendInviteResponse(triggerId, isAccepted);
    //    return ClientCallResult.Success;
    //}
}
