using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI.GameState_Management;
using WebAPI.GameState_Management.Game_State_Repository;
using WebAPI.GameTasks;
using WebAPI.GameTasks.Stations;
using WebAPI.Models;
using WebAPI.Models.DTOs;
using WebAPI.Room_template;
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
        private readonly IRoomRepository _roomRepository;
        public TheCrewController(ILogger<TheCrewController> logger,
            PlayerContext playerContext,
            IPlayerRepository playerRepository,
            IPlayerService playerService,
            IChatService chatService,
            IGameStateRepository gameStateRepository,
            IServiceProvider serviceProvider,
            IRoomRepository roomRepository)
        {
            _logger = logger;
            _playerRepository = playerRepository;
            _playerService = playerService;
            _playerContext = playerContext;
            _chatService = chatService;
            _gameStateRepository = gameStateRepository;
            _serviceProvider = serviceProvider;
            _roomRepository = roomRepository;

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
            //firstPLayer = new Player()
            //{
            //    Id = firstPLayer.Id,
            //    ActionPoints = firstPLayer.ActionPoints,
            //    CurrentChatRoomId = firstPLayer.CurrentChatRoomId,
            //    CurrentGameRoomId = firstPLayer.CurrentGameRoomId,
            //    GameId = firstPLayer.GameId,
            //    HealthPoints = 33,
            //    Name = "I was changed",
            //    Profession = firstPLayer.Profession,
            //    X = firstPLayer.X,
            //    Y = firstPLayer.Y,
            //    Z = firstPLayer.Z
            //};
        }
        [HttpGet]
        [Route("runcs")]
        public async Task<ActionResult> RunConstructor() { return Ok(); }


        [HttpGet]
        [Route("GetGameState")]
        public async Task<ActionResult<GameState>> GetGameState(Guid playerId, DateTime? lastTimeStamp)
        {
            var gameState = _gameStateRepository.GetPlayerGameState(playerId, lastTimeStamp);
            return Ok(gameState);
        }


        [HttpPut]
        [Route("TryExecuteGameTask")]
        public async Task<ActionResult<string>> GameTask(Guid playerId, GameTaskCode taskCode, [FromBody] Dictionary<string, string> parameters)
        { // ben : probleme de transfert avec le dictionnaire, demander si je peux pas mettre un string a la place 
          //demander multiple FromBody ou jsp 

            // GameTaskCode taskCode2 = (GameTaskCode)Convert.ToInt32(taskCode);

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

            _playerContext.PrivateChatRooms.Any(x => x.RoomId == invitation.RoomId && x.ParticipantId == invitation.ToPlayerId ); // should be able to replace those two horrible queries

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
        public async Task<ActionResult<string>> ChangeRoom(Guid playerId, string targetRoomName)
        {
            var player = _playerRepository.GetPlayer(playerId);

            var currentRoom = _playerContext.Rooms.First(x => x.Id == player.CurrentGameRoomId);

            var targetRoom = _playerContext.Rooms.First(x => x.Name == targetRoomName);

            bool connectionExists = _playerContext.AdjacentRooms.FirstOrDefault(x => x.RoomId == currentRoom.Id && x.AdjacentId == targetRoom.Id) is not null;

            if (connectionExists)
            {
                player.CurrentGameRoomId = targetRoom.Id;
                _playerContext.SaveChanges();

                return Ok("Tu as change de room!");
            }

            return Ok("Eat my shorts, the room does not exist.");
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
                GameId = defaultGameGuid,
                Name = "testRoom",
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

            _roomRepository.CreateNewGameRooms();

            _playerContext.Players.Add(fredPlayerModel);

            _playerContext.SaveChanges();
            return Ok();
        }


        [HttpPut]
        [Route("CreateNewGame")] // replaces defaultvalues
        public async Task<ActionResult> CreateNewGame()
        {
            // Init rooms
            _roomRepository.CreateNewGameRooms();

            //Guids
            Guid defaultGameGuid = new Guid("DE74B055-BA84-41A2-BAEA-4E380293E227");
            Guid defaultPlayer1Guid = new Guid("7E7B80A5-D7E2-4129-A4CD-59CF3C493F7F");
            Guid defaultplayer2guid = new Guid("b3543b2e-cd81-479f-b99e-d11a8aab37a0");
            //Players
            Player fredPlayerModel = new Player()
            {
                ActionPoints = 10,
                CurrentChatRoomId = defaultGameGuid,
                CurrentGameRoomId = _playerContext.Rooms.First().Id,
                GameId = defaultGameGuid,
                HealthPoints = 10,
                Id = defaultPlayer1Guid,
                Name = "Fred",
                Profession = Enums.ProfessionType.Commander,
                X = 0,
                Y = 0,
                Z = 0,
            };
            Player benPlayerModel = new Player()
            {
                ActionPoints = 4,
                CurrentChatRoomId = defaultGameGuid,
                CurrentGameRoomId = _playerContext.Rooms.First().Id,
                GameId = defaultGameGuid,
                HealthPoints = 5,
                Id = defaultplayer2guid,
                Name = "Ben",
                Profession = Enums.ProfessionType.Commander,
                X = 0,
                Y = 0,
                Z = 0,

            };
            _playerContext.Players.Add(fredPlayerModel);
            _playerContext.Players.Add(benPlayerModel);

            //Messages
            Message sampleMessage = new Message()
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                GameId = defaultGameGuid,
                RoomId = defaultGameGuid,
                Name = "Fred",
                Text = "Testoman!"
            };
            _playerContext.Messages.Add(sampleMessage);

            //Stations
            Station station = new Station()
            {
                Id = Guid.NewGuid(),
                Name = "CookStation1", // pour faire la diff ds unity, va avoir besoin de template hehelol
                GameTaskCode = GameTaskCode.Cook,
                GameId = defaultGameGuid,
                SerializedStation = JsonConvert.SerializeObject(new CookStationProperties()
                {
                    MoneyMade = 5,
                    State = Enums.State.Pristine
                }),
            };
            _playerContext.Station.Add(station);

            _playerContext.SaveChanges();
            return Ok();
        }
    }
}