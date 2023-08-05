using Microsoft.EntityFrameworkCore;
using Shared_Resources.Entities;
using Shared_Resources.Models;
using WebAPI.Interfaces;

namespace WebAPI.Services;

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

        _ = _playerContext.PrivateChatRoomParticipants.Add(pair);
        _ = await _playerContext.SaveChangesAsync();

        return ClientCallResult.Success;
    }

    public async Task<ClientCallResult> LeaveChatRoomAsync(Guid playerId, Guid roomToLeave)
    {
        var t = await _playerContext.PrivateChatRoomParticipants.FirstAsync(x => x.RoomId == roomToLeave && x.ParticipantId == playerId);
        _ = _playerContext.PrivateChatRoomParticipants.Remove(t);
        _ = await _playerContext.SaveChangesAsync();
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

        _ = _playerContext.Messages.Add(message);
        _ = await _playerContext.SaveChangesAsync();
    }
}
