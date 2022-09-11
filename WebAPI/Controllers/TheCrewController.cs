using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI.Models;
using WebAPI.Rooms;
using System.Linq;
using WebAPI.Services.ChatService;
using WebAPI.GameState_Management;
using WebAPI.GameState_Management.Game_State_Repository;
using WebAPI.GameTasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TheCrewController : ControllerBase
    {
        private readonly ILogger<TheCrewController> _logger;
        private readonly IChatService _chatService;
        private readonly IPlayerRepository _playerRepository;
        private readonly IPlayerService _playerService;
        private readonly IGameStateRepository _gameStateRepository;
        private readonly PlayerContext _playerContext;
        private readonly IServiceProvider _serviceProvider;

        public TheCrewController(ILogger<TheCrewController> logger,
            PlayerContext playerContext,
            IPlayerRepository playerRepository,
            IPlayerService playerService,
            IChatService chatService,
            IGameStateRepository gameStateRepository,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _playerRepository = playerRepository;
            _playerService = playerService;
            _playerContext = playerContext;
            _chatService = chatService;
            _gameStateRepository = gameStateRepository;
            _serviceProvider = serviceProvider;

            //Guid fredGud = new Guid("F0415BB0-5F22-4E79-1D4C-08DA5F69A35F");

            //Guid gameId = new Guid("F76A4822-8D96-4073-E7F8-08DA70D59137");
            //var fred = _playerRepository.GetPlayerByName("fred");
            //var ben = _playerRepository.GetPlayerByName("ben");

            //_playerContext.SaveChanges();
        }

        // [HttpGet(Name = "GetLocalPlayer")]
        [HttpGet]
        [Route("GetPlayers")]
        public async Task<ActionResult<List<Player>>> GetPlayers()
        {
            //return myPlayerContext.Players.ToList();
            var players = _playerContext.Players.ToList();
            //  var players = Task.FromResult(this._playerContext.Players.ToList()).ConfigureAwait(false);
            return Ok(players);
        }

        [HttpGet]
        [Route("DoNothing")]
        public async void DoNothing()
        {

        }

        [HttpGet]
        [Route("GetGameState")]
        public async Task<ActionResult<GameState>> GetGameState(Guid playerId, DateTime? lastTimeStamp)
        {
            var gameState = _gameStateRepository.GetPlayerGameState(playerId, lastTimeStamp);
            return Ok(gameState);
        }

        [HttpPost]
        [Route("GameTask")]
        public async Task<ActionResult<string>> GameTask(Guid playerId, GameTaskCode taskCode, Dictionary<string, string> parameters)
        {
            var gameState = _gameStateRepository.GetPlayerGameState(playerId, null);
            if (gameState == null)
            {
                return NotFound($"The player {playerId} was not found");
            }

            var context = new GameTaskContext
            {
                GameState = gameState,
                Parameters = parameters ?? new Dictionary<string, string>()
            };

            Type gameTaskType = GameTaskTypeSelector.GetGameTaskType(taskCode);
            var gameTask = _serviceProvider.GetService(gameTaskType) as IGameTask;

            var result = gameTask.Validate(context);
            if (!result.IsValid)
            {
                return Ok(result.ErrorMessage);
            }

            gameTask.Execute(context);
            return Ok($"The task {taskCode} was executed successfully.");
        }

        [HttpPut]
        [Route("SwitchRooms")]
        public async Task<ActionResult> SwitchRoom([FromBody] Player player, RoomType desiredRoomTransition)
        {
            RoomType playerRoomType = player.CurrentRoom;
            List<RoomType> adjacentRooms = AdjacentRooms.dic.GetValueOrDefault(playerRoomType);

            if (adjacentRooms == null) throw new NotImplementedException($"This was null : {nameof(adjacentRooms)}");

            bool isAdjacent = adjacentRooms.Contains(playerRoomType);

            if (isAdjacent)
            {
                var requestingUserInContext = _playerRepository.GetPlayerById(player.Id);
                requestingUserInContext.CurrentRoom = desiredRoomTransition;
                _playerContext.SaveChanges();
                return Ok(requestingUserInContext);
            }

            return Ok(player);
        }

        //[HttpPut]
        //[Route("CookTask")]
        //public async Task<ActionResult<string>> CookTask([FromBody] Player player) // F0415BB0-5F22-4E79-1D4C-08DA5F69A35F
        //{
        //}

        [HttpGet]
        [Route("GetPlayerNames")]
        public async Task<ActionResult<List<string>>> GetPlayerNames()
        {
            var names = _playerRepository.GetPlayerNames();
            return Ok(names);
        }

        [HttpGet]
        [Route("GetPlayerByName")]
        public async Task<ActionResult<Player>> GetPlayerByName(string name)
        {
            var player = await Task.FromResult(_playerRepository.GetPlayerByName(name)).ConfigureAwait(false);
            return Ok(player);
        }

        [HttpPut] // put = update, post = creation
        [Route("UpdatePositionByPlayerModel")]
        public async Task<ActionResult> UpdatePositionByPlayerModel([FromBody] Player UnityPlayerModel) // va dependre de comment je manage les data
        {
            _playerService.UpdatePlayerPosition(UnityPlayerModel);
            return Ok();
        }

        [HttpGet] // must work privateRooms now.
        [Route("GetMessages")]
        public async Task<ActionResult<List<Message>>> GetMessages(string playerGuid) // returns messages in current room
        {
            Guid playerId = new Guid(playerGuid);
            var player = _playerRepository.GetPlayerById(playerId);
            var currentRoomMessages = _playerContext.Messages.Where(message => message.RoomId == player.CurrentChatRoomId).ToList();

            return Ok(currentRoomMessages);
        }

        [HttpPut]
        [Route("PutNewMessageToServer")]
        public async Task<ActionResult> PutNewMessageToServer(string guid, string roomId, string receivedMessage)
        {
            var player = _playerRepository.GetPlayerById(new Guid(guid));

            var message = new Message()
            {
                Id = Guid.NewGuid(),
                GameId = player.GameId,
                Text = receivedMessage,
                Name = player.Name,
                RoomId = new Guid(roomId),
                Created = DateTime.UtcNow,
            };

            _playerContext.Messages.Add(message);
            _playerContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("GetPlayersCurrentGame")]
        public async Task<ActionResult<List<Player>>> GetPlayersCurrentGame(string playerGuid)
        {
            Guid playerId = new Guid(playerGuid);
            var player = _playerRepository.GetPlayerById(playerId);

            var currentPlayers = _playerContext.Players.Where(apiPlayer => apiPlayer.GameId == player.GameId).ToList();
            return Ok(currentPlayers);
        }

        [HttpGet]
        [Route("GetPlayersCurrentGameChatRoom")]
        public async Task<ActionResult<List<Player>>> GetPlayersCurrentGameChatRoom(string playerGuid)
        {
            Guid playerId = new Guid(playerGuid);
            var player = _playerRepository.GetPlayerById(playerId);

            if (player.GameId == player.CurrentChatRoomId) // si global, get tous les joueurs dans global 
            {
                var currentPlayers = _playerContext.Players.Where(apiPlayer => apiPlayer.GameId == player.GameId).ToList();
                return Ok(currentPlayers);
            }

            else // si dans une privee, get tous les joueurs participant au private
            {
                var playersInRoomGuids = _playerContext.PrivateChatRooms.Where(pair => pair.RoomId == player.CurrentChatRoomId)
                    .Select(guid => guid.ParticipantId).ToList();
                var playersInRoom = _playerContext.Players.Where(qPlayer => playersInRoomGuids.Contains(qPlayer.Id)).ToList();

                return Ok(playersInRoom);
            }
            throw new Exception("shouldnt return nothing");
            return null;
        }

        [HttpPut]
        [Route("PutCreateChatRoom")] // techniquement ce sera la guid cree par le unity client que je vais utiliser donc faudra reecrire cela 
        public async Task<ActionResult<string>> PutCreatePrivateChatRoom(string playerGuid) // UpdateOrCreate ? pour 
        {
            Guid playerId = new Guid(playerGuid);
            var player = _playerRepository.GetPlayerById(playerId);

            var privateRoomPair = new PrivateChatRoomParticipant
            {
                Id = Guid.NewGuid(),
                ParticipantId = playerId,
                RoomId = Guid.NewGuid(),
            };
            _playerContext.PrivateChatRooms.Add(privateRoomPair);
            _playerContext.SaveChanges();
            return Ok("lol");
        }

        [HttpPut]
        [Route("InviteToChatRoom")]
        public async Task<ActionResult<string>> PutInviteToChatRoom([FromBody] PrivateInvitation invitation)
        {
            Player requestingPlayer = _playerRepository.GetPlayerById(invitation.FromPlayerId);

            //must complete the invitedusername
            invitation.ToPlayerName = _playerRepository.GetPlayerById(invitation.ToPlayerId).Name;

            // Dont add invitation if player already in room
            foreach (var privateRoomPair in _playerContext.PrivateChatRooms)
            {
                bool isUserAlreadyPresent = privateRoomPair.RoomId == invitation.RoomId;
                bool isRoomAlreadyPresent = privateRoomPair.ParticipantId == invitation.ToPlayerId;

                if (isUserAlreadyPresent && isRoomAlreadyPresent)
                {
                    return Ok("AlreadyInRoom");
                }
            }

            // dont add invitation if invitation already exists
            foreach (var invite in _playerContext.Invitations)
            {
                bool isUserAlreadyPresent = invite.ToPlayerId == invitation.ToPlayerId;
                bool isRoomAlreadyPresent = invite.RoomId == invitation.RoomId;

                if (isUserAlreadyPresent && isRoomAlreadyPresent)
                {
                    return Ok("Already invited to the same room");
                }
            }

            _playerContext.Invitations.Add(invitation);
            _playerContext.SaveChanges();
            return Ok("yeah");
        }

        [HttpGet]
        [Route("GetPendingInvitations")]
        public async Task<ActionResult<PrivateInvitation>> GetPendingInvitations(string playerGuid)
        {
            Guid playerId = new Guid(playerGuid);
            var player = _playerRepository.GetPlayerById(playerId);

            // var firstPendingInvite = _playerContext.Invitations.Where(invite => invite.InvitedGuid == player.Id).FirstOrDefault();

            var firstPendingInvite = _playerContext.Invitations.FirstOrDefault(invi => invi.ToPlayerId == playerId);
            if (firstPendingInvite == null)
            {
                return new PrivateInvitation() { Id = Guid.Empty }; // no invites found
            }

            return firstPendingInvite;
        }

        [HttpPut]
        [Route("SendInvitationResponse")]
        public async Task<string> SendInvitationResponse([FromBody] PrivateInvitation invitation)
        {
            var invite = _playerContext.Invitations.FirstOrDefault(invite => invite.Id == invitation.Id);

            invite.FromPlayerId = invitation.FromPlayerId;
            invite.FromPlayerName = invitation.FromPlayerName;
            invite.ToPlayerName = invitation.ToPlayerName;
            invite.ToPlayerId = invitation.ToPlayerId;
            invite.IsAccepted = invitation.IsAccepted;
            invite.RequestFulfilled = true;
            invite.RoomId = invitation.RoomId;

            var invitingPlayer = _playerRepository.GetPlayerById(invite.FromPlayerId);

            if (invite.IsAccepted)
            {
                //var CurrentRoomGuids = _playerContext.PrivateChatRooms.Select(pair => pair.RoomId);
                //bool currentRoomDoesNotExist = !CurrentRoomGuids.Contains(invite.InvitedRoomGuid);
                //if (currentRoomDoesNotExist)
                //{
                //    var newPrivate
                //}

                //specify that the player is in that room

                var privatePair = new PrivateChatRoomParticipant()
                {
                    Id = Guid.NewGuid(),
                    RoomId = invitation.RoomId,
                    ParticipantId = invite.ToPlayerId,

                };

                // I dont need the invitation after having resolved it 
                _playerContext.Invitations.Remove(invite);

                _playerContext.PrivateChatRooms.Add(privatePair);
            }

            _playerContext.SaveChanges();

            return $"You successfully invited {invitingPlayer.Name} to your private chat room. ";
        }


        [HttpGet]
        [Route("GetPrivateRoomGuids")]
        public async Task<ActionResult<List<Guid>>> GetPrivateRoomGuids(string playerGuid)
        {
            Guid playerId = new Guid(playerGuid);
            var player = _playerRepository.GetPlayerById(playerId);

            var playerRoomPairs = _playerContext.PrivateChatRooms.Where(pair => pair.ParticipantId == playerId);

            var privateRoomIds = playerRoomPairs.Select(pair => pair.RoomId);
            return Ok(privateRoomIds.ToList());
        }

        [HttpPut]
        [Route("UpdateCurrentRoomId")]
        public async Task<ActionResult<string>> UpdateCurrentRoomId(string playerGuid, string currentChatRoom)
        {
            Guid playerId = new Guid(playerGuid);
            var player = _playerRepository.GetPlayerById(playerId);

            Guid currentChatRoomId = new Guid(currentChatRoom);

            player.CurrentChatRoomId = currentChatRoomId;
            _playerContext.SaveChanges();
            return Ok("success");
        }


        [HttpDelete]
        [Route("ClearInvitations")]
        public async Task<ActionResult> ClearInvitations()
        {
            foreach (var invite in _playerContext.Invitations)
            {
                _playerContext.Invitations.Remove(invite);
            }
            // kill cook 
            foreach (var pair in _playerContext.PrivateChatRooms)
            {
                _playerContext.PrivateChatRooms.Remove(pair);
            }
            _playerContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("AddPlayerRoomPair")]
        public async Task<ActionResult> AddPlayerRoomPair(string playerGuid, string newRoomGuid)
        {
            Guid playerid = new Guid(playerGuid);
            Guid newRoomid = new Guid(newRoomGuid);

            var pair = new PrivateChatRoomParticipant()
            {
                Id = Guid.NewGuid(),
                ParticipantId = playerid,
                RoomId = newRoomid
            };

            _playerContext.PrivateChatRooms.Add(pair);
            _playerContext.SaveChanges();

            return Ok();
        }
    }
}