using WebAPI.GameState_Management.Game_State_Service;
using WebAPI.Models;

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
            var player = _playerRepository.GetPlayerById(playerId); // delete roomid, add private chatroom
            var gameState = new GameState() // passer par Id la prochaine fois 
            {                               // veux toujours passer le minimum quand tu communiques entre des fonctions 
                Player = player,            // objet pas modified 
                Invitations = _playerRepository.GetPlayerInvitations(player),
                NewMessages = GetNewMessages(lastTimeStamp, player),
                Players = _playerRepository.GetPlayersInCurrentGame(player),
                TimeStamp = DateTime.UtcNow,
                PrivateChatRooms = GetChatRoomsWithMainPlayerInIt(player),
            };
            return gameState;
        }

        public List<PrivateChatRoomParticipant> GetChatRoomsWithMainPlayerInIt(Player mainPlayer)
        {
            //var roomsTheMainPlayerIsIn = _playerContext.PrivateChatRooms.Where(pair => pair.ParticipantId == mainPlayer.Id);

            //List<PrivateChatRoomParticipant> participantsInSameRoomAsPlayer =
            //    roomsTheMainPlayerIsIn.Join(_playerContext.PrivateChatRooms,
            //    chatRoomWithMainPlayer => chatRoomWithMainPlayer.RoomId,
            //    chatRoomWithAnyPlayer => chatRoomWithAnyPlayer.RoomId,
            //    (p1, p2) => new PrivateChatRoomParticipant()
            //    {
            //        Id = p2.Id,
            //        ParticipantId = p2.ParticipantId,
            //        RoomId = p2.RoomId,
            //    })
            //    .Where(privateRoom=> privateRoom.ParticipantId != mainPlayer.Id).ToList();

            var roomsTheMainPlayerIsIn = _playerContext.PrivateChatRooms.Where(pair => pair.ParticipantId == mainPlayer.Id).ToList();

            var chatRoomIds = roomsTheMainPlayerIsIn.Select(chatRoom => chatRoom.RoomId).ToList();


            var participantsInSameRoomAsPlayer = _playerContext.PrivateChatRooms.Where(room => chatRoomIds.Contains(room.RoomId)).ToList(); // qqch marche pas
            var excludeMainPlayer = participantsInSameRoomAsPlayer.Where(room => room.ParticipantId != mainPlayer.Id).ToList();

            return participantsInSameRoomAsPlayer;
        }

        public List<Message> GetNewMessages(DateTime? lastTimeStamp, Player player)
        {
            bool hasReceivedMessages = lastTimeStamp != null;
            return !hasReceivedMessages
                ? GetMessagesFromGameId(player.GameId)
                : GetMessagesAfterTimeStamp(lastTimeStamp, player.GameId);
        }

        public List<Message> GetMessagesFromGameId(Guid gameId)
        {
            var allMessages = _playerContext.Messages.Where(mess => mess.GameId == gameId).ToList();
            return allMessages;
        }

        public List<Message> GetMessagesAfterTimeStamp(DateTime? timeStamp, Guid gameId)
        {
            List<Message> messagesAfterTimeStamp = new List<Message>();
            foreach (Message message in _playerContext.Messages)
            {
                bool isAfterTimeStamp = message.Created > timeStamp;
                if (!isAfterTimeStamp) continue;

                bool isPlayerGame = message.GameId == gameId;
                if (!isPlayerGame) continue;

                messagesAfterTimeStamp.Add(message);
            }
            return messagesAfterTimeStamp;
        }
    }
}
