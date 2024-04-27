using Microsoft.EntityFrameworkCore;
using Northwest.Persistence;
using Northwest.Persistence.Entities;

namespace Northwest.Domain.Services;

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
        var playerid = new Guid(playerGuid);
        var newRoomid = new Guid(newRoomGuid);

        var pair = new PrivateChatRoomParticipant()
        {
            Id = Guid.NewGuid(),
            ParticipantId = playerid,
            RoomId = newRoomid
        };

        _playerContext.PrivateChatRoomParticipants.Add(pair);
        await _playerContext.SaveChangesAsync();
    }

    public async Task LeaveChatRoomAsync(Guid playerId, Guid roomToLeave)
    {
        var t = await _playerContext.PrivateChatRoomParticipants.FirstAsync(x => x.RoomId == roomToLeave && x.ParticipantId == playerId);
        _playerContext.PrivateChatRoomParticipants.Remove(t);
        await _playerContext.SaveChangesAsync();
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
}
