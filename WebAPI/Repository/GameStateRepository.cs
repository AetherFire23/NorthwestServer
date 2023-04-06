using WebAPI.Game_Actions;
using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using Shared_Resources.DTOs;
using WebAPI.Interfaces;
using Shared_Resources.Models;

namespace WebAPI.Repository
{
    public class GameStateRepository : IGameStateRepository
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly PlayerContext _playerContext;
        public GameStateRepository(PlayerContext playerContext, IPlayerRepository playerRepository, IRoomRepository roomRepository)
        {
            _playerContext = playerContext;
            _playerRepository = playerRepository;
            _roomRepository = roomRepository;
        }

        public GameState GetPlayerGameState(Guid playerId, DateTime? lastTimeStamp)
        {
            //temp
            //Player player = _playerContext.Players.FirstOrDefault(player => player.Id == playerId);
            Player player = _playerRepository.GetPlayer(playerId);

            PlayerDTO playerDTO = _playerRepository.MapPlayerDTO(playerId);
            List<Message> newMessages = GetNewMessages(lastTimeStamp, playerDTO.GameId);
            List<Player> players = _playerRepository.GetPlayersInGame(playerDTO.GameId);
            DateTime timeStamp = DateTime.UtcNow;
            List<PrivateChatRoomParticipant> chatRoomParticipants = GetChatRoomsWithMainPlayerInIt(playerDTO.Id);

            //string serializedLayout = _playerContext.Landmass.First(l => l.GameId == playerDTO.GameId).SerializedLandmassLayout;
            RoomDTO roomDTO = _roomRepository.GetRoomDTO(player.CurrentGameRoomId);
            var gameState = new GameState()
            {
                PlayerDTO = playerDTO,
                TriggerNotifications = _playerRepository.GetTriggerNotifications(playerId, lastTimeStamp),
                NewMessages = newMessages,
                Players = players,
                TimeStamp = timeStamp,
                PrivateChatRooms = chatRoomParticipants,
                Room = roomDTO,
                Logs = _playerRepository.GetAccessibleLogs(playerId, lastTimeStamp),
                Rooms = GetAllRoomsInGame(player.GameId),
                SerializedLayout = string.Empty,
            };

            return gameState;
        }

        public List<RoomDTO> GetAllRoomsInGame(Guid gameId)
        {
            var roomsInGame = _playerContext.Rooms.Where(x => x.GameId == gameId).ToList();

            List<RoomDTO> roomDTOs = roomsInGame.Select(x => _roomRepository.GetRoomDTO(x.Id)).ToList();

            return roomDTOs;
        }

        public List<PrivateChatRoomParticipant> GetChatRoomsWithMainPlayerInIt(Guid playerID)
        {
            var roomsTheMainPlayerIsIn = _playerContext.PrivateChatRooms.Where(pair => pair.ParticipantId == playerID).ToList();
            var chatRoomIds = roomsTheMainPlayerIsIn.Select(chatRoom => chatRoom.RoomId).ToList();
            var chatRoomWithMainPlayer = _playerContext.PrivateChatRooms.Where(room => chatRoomIds.Contains(room.RoomId)).ToList(); // qqch marche pas
            var excludeMainPlayer = chatRoomWithMainPlayer.Where(room => room.ParticipantId != playerID).ToList();

            return chatRoomWithMainPlayer;
        }

        public List<Message> GetNewMessages(DateTime? lastTimeStamp, Guid gameId)
        {
            bool hasReceivedMessages = lastTimeStamp != null;
            return !hasReceivedMessages
                   ? GetMessagesFromGameId(gameId)
                   : GetMessagesAfterTimeStamp(lastTimeStamp, gameId);
        }

        public List<Message> GetMessagesFromGameId(Guid gameId)
        {
            var allMessages = _playerContext.Messages.Where(mess => mess.GameId == gameId).ToList();
            return allMessages;
        }

        public List<Message> GetMessagesAfterTimeStamp(DateTime? timeStamp, Guid gameId)
        {
            return _playerContext.Messages
                .Where(message => message.Created > timeStamp
                      && message.GameId == gameId).ToList();
        }
    }
}
