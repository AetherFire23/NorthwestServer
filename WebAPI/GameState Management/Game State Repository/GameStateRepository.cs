using WebAPI.Game_Actions;
using WebAPI.GameState_Management.Game_State_Service;
using WebAPI.Models;
using WebAPI.Models.DTOs;

namespace WebAPI.GameState_Management.Game_State_Repository
{
    public class GameStateRepository : IGameStateRepository
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly PlayerContext _playerContext;
        public GameStateRepository(PlayerContext playerContext, IPlayerRepository playerRepository)
        {
            _playerContext = playerContext;
            _playerRepository = playerRepository;
        }

        public GameState GetPlayerGameState(Guid playerId, DateTime? lastTimeStamp) // va appeler a etre modified pour le cheating car jenvoie tout en meme temps 
        {
            //temp
            Player player = _playerContext.Players.First(player => player.Id == playerId);

            PlayerDTO playerDTO = _playerRepository.MapPlayerDTO(playerId); // delete roomid, add private chatroom
            //List<PrivateInvitation> invitations = _playerRepository.GetPlayerInvitations(playerDTO.Id);
            List<Message> newMessages = GetNewMessages(lastTimeStamp, playerDTO.GameId);
            List<Player> players = _playerRepository.GetPlayersInCurrentGame(playerDTO.GameId);
            DateTime timeStamp = DateTime.UtcNow;
            List<PrivateChatRoomParticipant> chatRoomParticipants = GetChatRoomsWithMainPlayerInIt(playerDTO.Id);
            RoomDTO roomDTO = _playerRepository.GetRoomDTO(player.CurrentGameRoomId); // bug ici ofc car je nai pa de room mesemble


            var gameState = new GameState()
            {
                PlayerDTO = playerDTO,
                TriggerNotifications = _playerRepository.GetTriggerNotifications(playerId, lastTimeStamp),
                NewMessages = newMessages,
                Players = players,
                TimeStamp = timeStamp,
                PrivateChatRooms = chatRoomParticipants,
                Room = roomDTO
            };
            return gameState;
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
