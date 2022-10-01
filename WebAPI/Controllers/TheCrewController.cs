using Microsoft.AspNetCore.Mvc;
using WebAPI.GameState_Management;
using WebAPI.GameState_Management.Game_State_Repository;
using WebAPI.GameTasks;
using WebAPI.Models;
using WebAPI.Models.DTOs;
using WebAPI.Services.ChatService;

namespace WebAPI.Controllers
{
    // Fait roomDTO, playerDTO
    // Fait des items utilisables
    // ItemType

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

            //  Guid defaultPlayer1Guid = new Guid("7E7B80A5-D7E2-4129-A4CD-59CF3C493F7F");

            //var p =  _playerRepository.GetPlayer(defaultPlayer1Guid);

            // _playerContext.Items.Add(new Item()
            // {
            //     Id = Guid.NewGuid(),
            //     ItemType = Enums.ItemType.Wrench,
            //     OwnerId = defaultPlayer1Guid,
            // });

            // _playerContext.SaveChanges();

            // var pDTo = _playerRepository.MapPlayerDTO(defaultPlayer1Guid);
        }


        [HttpGet]
        [Route("GetGameState")]
        public async Task<ActionResult<GameState>> GetGameState(Guid playerId, DateTime? lastTimeStamp)
        {
            var gameState = _gameStateRepository.GetPlayerGameState(playerId, lastTimeStamp);
            return Ok(gameState);
        }

        [HttpPut]
        [Route("UpdateCurrentRoomId")]
        public async Task<ActionResult<string>> UpdateCurrentRoomId(string playerGuid, string currentChatRoom)
        {
            Guid playerId = new Guid(playerGuid);
            var player = _playerRepository.GetPlayer(playerId);

            Guid currentChatRoomId = new Guid(currentChatRoom);

            player.CurrentChatRoomId = currentChatRoomId;
            _playerContext.SaveChanges();
            return Ok("success");
        }

        [HttpPost]
        [Route("TryExecuteGameTask")]
        public async Task<ActionResult<string>> GameTask(Guid playerId, GameTaskCode taskCode, [FromBody] Dictionary<string, string> parameters)
        { // ben : probleme de transfert avec le dictionnaire, demander si je peux pas mettre un string a la place 
            //demander multiple FromBody ou jsp 
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

        [HttpPut] // put = update, post = creation
        [Route("UpdatePositionByPlayerModel")]
        public async Task<ActionResult> UpdatePositionByPlayerModel([FromBody] Player UnityPlayerModel) // va dependre de comment je manage les data
        {
            _playerService.UpdatePlayerPosition(UnityPlayerModel);
            return Ok();
        }

        [HttpPut]
        [Route("PutNewMessageToServer")]
        public async Task<ActionResult> PutNewMessageToServer(string guid, string roomId, string receivedMessage)
        {
            var player = _playerRepository.GetPlayer(new Guid(guid));

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

        [HttpPut]
        [Route("PutCreateChatRoom")] // pense obsolet 
        public async Task<ActionResult<string>> PutCreatePrivateChatRoom(string playerGuid) // UpdateOrCreate ? pour 
        {
            Guid playerId = new Guid(playerGuid);
            var player = _playerRepository.GetPlayer(playerId);

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
            Player requestingPlayer = _playerRepository.GetPlayer(invitation.FromPlayerId);

            //must complete the invitedusername
            invitation.ToPlayerName = _playerRepository.GetPlayer(invitation.ToPlayerId).Name;

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

            var invitingPlayer = _playerRepository.GetPlayer(invite.FromPlayerId);

            if (invite.IsAccepted)
            {
                //specify that the player is in that room
                var privatePair = new PrivateChatRoomParticipant()
                {
                    Id = Guid.NewGuid(),
                    RoomId = invitation.RoomId,
                    ParticipantId = invite.ToPlayerId,

                };

                // I dont need the invitation after having resolved it 
                _playerContext.Invitations.Remove(invite); // Removed quand yer accepted mais pas removed quand yer refuse ? possibilite de bug 

                _playerContext.PrivateChatRooms.Add(privatePair);
            }

            _playerContext.SaveChanges();

            return $"You successfully invited {invitingPlayer.Name} to your private chat room. ";
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
        [Route("AddPlayerRoomPair")] // pense c'est lui utilise par unity
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

        [HttpPut]
        [Route("TransferItem")]
        public async Task<ActionResult> TransferItem(Guid ownerId, Guid targetId, Guid itemId) // pourrait devenir une method dans le service
        {
            Item? selectedItem = _playerContext.Items.First(i => i.Id == itemId);
            selectedItem.OwnerId = targetId;
            _playerContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("ChangeRoom")]
        public async Task<ActionResult> ChangeRoom(Guid targetRoomId) // pourrait devenir une method dans le service
        {
            var p = _playerRepository.GetPlayer(targetRoomId);
            p.CurrentGameRoomId = targetRoomId;

            _playerContext.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("RunConstructor")]
        public async void DoNothing()
        {

        }

        [HttpPut]
        [Route("AddDefaultValues")]
        public async Task<ActionResult> GetGameState()
        {
            
            Guid defaultGameGuid = new Guid("DE74B055-BA84-41A2-BAEA-4E380293E227");
            Guid defaultPlayer1Guid = new Guid("7E7B80A5-D7E2-4129-A4CD-59CF3C493F7F");
            Guid firstRoomId = new Guid("57D88036-A7C8-448D-B2D9-0842E83D8231");
            Guid defaultItemId = new Guid("F0970083-468F-40AF-9BAE-F333C76A5D92");
            Item item = new Item()
            {
                Id = defaultItemId,
                ItemType = Enums.ItemType.Wrench,
                OwnerId = defaultPlayer1Guid,
            };
            _playerContext.Items.Add(item);

            Room defaultRoom = new Room()
            {
                Id = firstRoomId,
                RoomType = RoomType.Start
            };
            _playerContext.Rooms.Add(defaultRoom);

            Player fredPlayerModel = new Player()
            {
                ActionPoints = 10,
                CurrentChatRoomId = defaultGameGuid,
                CurrentGameRoomId = defaultRoom.Id,
                GameId = defaultGameGuid,
                HealthPoints = 10,
                Id = defaultPlayer1Guid,
                Name = "Fred",
                Profession = Enums.ProfessionType.Commander,
                X = 0,
                Y = 0,
                Z = 0,
            };
            _playerContext.Players.Add(fredPlayerModel);

            //PlayerDTO playerDTO = new PlayerDTO()
            //{
            //    Id = fredPlayerModel.Id,
            //    ActionPoints = fredPlayerModel.ActionPoints,
            //    GameId = fredPlayerModel.GameId,
            //    HealthPoints = fredPlayerModel.HealthPoints,
            //    Items = _playerContext.Items.Where(i=> i.OwnerId == fredPlayerModel.Id).ToList(),
            //    Name = fredPlayerModel.Name,
            //    Skills = _playerContext.Skills.Where(s => s.OwnerId == fredPlayerModel.Id).Select(s=> s.SkillType).ToList(),
            //    X = fredPlayerModel.X,
            //    Y = fredPlayerModel.Y,
            //    Z = fredPlayerModel.Z

            //};

            _playerContext.SaveChanges();
            return Ok();
        }
    }
}