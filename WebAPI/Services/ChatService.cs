using Microsoft.EntityFrameworkCore;
using Shared_Resources.Entities;

namespace WebAPI.Services;

public class ChatService
{
    private readonly PlayerContext _playerContext;
    private readonly PlayerRepository _playerRepository;
    public ChatService(PlayerContext playerContext, PlayerRepository playerRepository)
    {
        _playerContext = playerContext;
        _playerRepository = playerRepository;
    }

    public async Task CreateChatroomAsync(string playerGuid, string newRoomGuid)
    {
        Guid playerid = new Guid(playerGuid);
        Guid newRoomid = new Guid(newRoomGuid);

        PrivateChatRoomParticipant pair = new PrivateChatRoomParticipant()
        {
            Id = Guid.NewGuid(),
            ParticipantId = playerid,
            RoomId = newRoomid
        };

        _ = _playerContext.PrivateChatRoomParticipants.Add(pair);
        _ = await _playerContext.SaveChangesAsync();
    }

    public async Task LeaveChatRoomAsync(Guid playerId, Guid roomToLeave)
    {
        PrivateChatRoomParticipant t = await _playerContext.PrivateChatRoomParticipants.FirstAsync(x => x.RoomId == roomToLeave && x.ParticipantId == playerId);
        _ = _playerContext.PrivateChatRoomParticipants.Remove(t);
        _ = await _playerContext.SaveChangesAsync();
    }

    public async Task PutNewMessageToServerAsync(string guid, string roomId, string receivedMessage)
    {
        Player player = await _playerRepository.GetPlayerAsync(new Guid(guid));

        Message message = new Message()
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
