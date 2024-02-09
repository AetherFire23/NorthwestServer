using Microsoft.EntityFrameworkCore;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Models;

namespace WebAPI.Repositories;

public class GameStateRepository
{
    private readonly RoomRepository _roomRepository;
    private readonly PlayerRepository _playerRepository;
    private readonly PlayerContext _playerContext;
    public GameStateRepository(PlayerContext playerContext, PlayerRepository playerRepository, RoomRepository roomRepository)
    {
        _playerContext = playerContext;
        _playerRepository = playerRepository;
        _roomRepository = roomRepository;
    }

    public async Task<GameState> GetPlayerGameStateAsync(Guid playerId, DateTime? lastTimeStamp)
    {
        Player player = await _playerRepository.GetPlayerAsync(playerId);

        PlayerDto playerDTO = await _playerRepository.MapPlayerDTOAsync(playerId);
        List<Message> newMessages = await GetNewMessagesAsync(lastTimeStamp, playerDTO.GameId);
        List<Player> players = await _playerRepository.GetPlayersInGameAsync(playerDTO.GameId);
        List<PrivateChatRoomParticipant> chatRoomParticipants = await GetChatRoomsWithMainPlayerInItAsync(playerDTO.Id);
        List<RoomDTO> roomList = await GetAllRoomDTOSInGameAsync(playerDTO.GameId);

        List<Log> logs = await _playerRepository.GetAccessibleLogsForPlayer(playerId, player.GameId);
        List<PrivateChatRoom> privs = await GetPrivateChatRoomsAsync(player.Id);
        List<Station> stations = await _playerContext.Stations.Where(x => x.GameId == player.GameId).ToListAsync();

        GameState gameState = new GameState()
        {
            PlayerDTO = playerDTO,
            NewMessages = newMessages,
            Players = players,
            TimeStamp = DateTime.UtcNow,
            PrivateChatRoomParticipants = chatRoomParticipants,
            Logs = logs,
            Rooms = roomList,
            SerializedLayout = string.Empty,
            PrivateChatRooms = privs,
            Stations = stations,
        };

        return gameState;
    }

    public async Task<List<PrivateChatRoom>> GetPrivateChatRoomsAsync(Guid playerId)
    {
        List<PrivateChatRoom> rooms = await _playerContext.PrivateChatRooms.Join(_playerContext.PrivateChatRoomParticipants,
            p => p.Id,
            participant => participant.RoomId,
            (p, p2) => p).ToListAsync();

        return rooms;
    }

    public async Task<List<RoomDTO>> GetAllRoomDTOSInGameAsync(Guid gameId)
    {
        List<Room> roomsInGame = await _playerContext.Rooms.Where(x => x.GameId == gameId).ToListAsync();

        List<RoomDTO> roomDtos = new List<RoomDTO>();

        foreach (Room? room in roomsInGame)
        {
            RoomDTO roomDTO = await _roomRepository.GetRoomDTOAsync(room.Id);
            roomDtos.Add(roomDTO);
        }

        // var roomDTOs = await Task.WhenAll(roomsInGame.Select(async x => await _roomRepository.GetRoomDTOAsync(x.Id)));

        return roomDtos;
    }

    public async Task<List<PrivateChatRoomParticipant>> GetChatRoomsWithMainPlayerInItAsync(Guid playerID)
    {
        List<PrivateChatRoomParticipant> roomsTheMainPlayerIsIn = await _playerContext.PrivateChatRoomParticipants.Where(pair => pair.ParticipantId == playerID).ToListAsync();
        List<Guid> chatRoomIds = roomsTheMainPlayerIsIn.Select(chatRoom => chatRoom.RoomId).ToList();
        List<PrivateChatRoomParticipant> participantsInSameRoom = await _playerContext.PrivateChatRoomParticipants.Where(room => chatRoomIds.Contains(room.RoomId)).ToListAsync(); // qqch marche pas
        IEnumerable<PrivateChatRoomParticipant> excludeMainPlayer = participantsInSameRoom.Where(room => room.ParticipantId != playerID);

        return participantsInSameRoom;
    }

    public async Task<List<Message>> GetNewMessagesAsync(DateTime? lastTimeStamp, Guid gameId)
    {
        bool hasReceivedMessages = lastTimeStamp != null;
        List<Message> messages = !hasReceivedMessages
               ? GetMessagesFromGameId(gameId)
               : GetMessagesAfterTimeStamp(lastTimeStamp, gameId);

        return messages;
    }

    public List<Message> GetMessagesFromGameId(Guid gameId)
    {
        List<Message> allMessages = _playerContext.Messages.Where(mess => mess.GameId == gameId).ToList();
        return allMessages;
    }

    public List<Message> GetMessagesAfterTimeStamp(DateTime? timeStamp, Guid gameId)
    {
        return _playerContext.Messages
            .Where(message => message.Created > timeStamp
                  && message.GameId == gameId).ToList();
    }
}
