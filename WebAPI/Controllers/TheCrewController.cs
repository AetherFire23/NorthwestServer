using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI.Enums;
using WebAPI.Game_Actions;
using WebAPI.GameState_Management;
using WebAPI.GameState_Management.Game_State_Repository;
using WebAPI.GameTasks;
using WebAPI.GameTasks.Stations;
using WebAPI.Main_Menu.Models;
using WebAPI.Models;
using WebAPI.Repository;
using WebAPI.Room_template;
using WebAPI.Services.ChatService;

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
        private readonly IRoomRepository _roomRepository;
        private readonly IGameActionsRepository _gameActionsRepository;
        private readonly IMainMenuRepository _mainMenuRepository;

        public TheCrewController(ILogger<TheCrewController> logger,
            PlayerContext playerContext,
            IPlayerRepository playerRepository,
            IPlayerService playerService,
            IChatService chatService,
            IGameStateRepository gameStateRepository,
            IServiceProvider serviceProvider,
            IRoomRepository roomRepository,
            IGameActionsRepository gameActionsRepository,
            IMainMenuRepository mainMenuRepository)
        {
            _logger = logger;
            _playerRepository = playerRepository;
            _playerService = playerService;
            _playerContext = playerContext;
            _chatService = chatService;
            _gameStateRepository = gameStateRepository;
            _serviceProvider = serviceProvider;
            _roomRepository = roomRepository;
            _gameActionsRepository = gameActionsRepository;
            _mainMenuRepository = mainMenuRepository;
        }

        [HttpGet]
        [Route("runcs")]
        public async Task<ActionResult> RunConstructor() 
        { 
            return Ok(); 
        }

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

        [HttpPut] // put = update, post = creation
        [Route("UpdatePositionByPlayerModel")]
        public async Task<ActionResult> UpdatePositionByPlayerModel([FromBody] Player UnityPlayerModel) // va dependre de comment je manage les data
        {
            _playerService.UpdatePlayerPosition(UnityPlayerModel);
            return Ok();
        }

        [HttpPut]
        [Route("dwdwdw")]
        public async Task<ActionResult<ClientCallResult>> Test2()
        {
            var t = new ClientCallResult()
            {
                Success = true,
                SuccessMessage = "Win!",
            };
            return Ok(t);
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

        [HttpPut]
        [Route("TransferItem")] // me sers meme pas du ownerId
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

            var targetRoom = _playerContext.Rooms.First(x => x.Name == targetRoomName); // devrait pas etre avec le gameid aussi?

            bool connectionExists = _playerContext.AdjacentRooms.FirstOrDefault(x => x.RoomId == currentRoom.Id && x.AdjacentId == targetRoom.Id) is not null;

            if (connectionExists)
            {
                player.CurrentGameRoomId = targetRoom.Id;
                _playerContext.SaveChanges();

                _gameActionsRepository.ChangeRoomAction(player, currentRoom, targetRoom);
                // creer une nouvelle gameAction

                return Ok("Tu as change de room!"); 
            }

            return Ok("Eat my shorts, the room does not exist.");
        }

        [HttpGet]
        [Route("RunConstructor")]
        public async void DoNothing()
        {
            //Items 
            Guid defaultPlayer1Guid = new Guid("7E7B80A5-D7E2-4129-A4CD-59CF3C493F7F");

            Guid Item1 = Guid.NewGuid();
            _playerContext.Items.Add(new Item()
            {
                Id = Item1,
                ItemType = ItemType.Hose,
                OwnerId = defaultPlayer1Guid,
            });
            _playerContext.SaveChanges();
        }

        [HttpPut]
        [Route("AddDefaultValues")]
        public async Task<ActionResult> GetGameState()
        {

            Guid defaultGameGuid = new Guid("DE74B055-BA84-41A2-BAEA-4E380293E227");

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

            _playerContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("AddOrRemoveFriend")]
        public async Task<ActionResult> InteractWithFriends(Guid userId, FriendOptions option, [FromBody] FriendContext context)
        {
             switch (option)
            {
                case FriendOptions.Invite:
                    {
                        var targetUser = _playerContext.Users.First(x => x.Username == context.TargetName);
                        var targetId = targetUser.Id;
                        var targetName = targetUser.Username;
                        string extraProperties = JsonConvert.SerializeObject(new FriendContext()
                        {
                            CallerUserId = userId,
                            CallerUserName = _playerContext.Users.First(x => x.Id == userId).Username,
                            TargetId = targetId,

                        });
                        var newNotification = new MenuNotification()
                        {
                            MenuNotificationType = MenuNotificationType.FriendInvite,
                            Id = Guid.NewGuid(),
                            ToId = targetId,
                            ExtraProperties = extraProperties,
                            Handled = false,
                            Retrieved = false,
                            TimeStamp = DateTime.Now,
                        };
                        _playerContext.MenuNotifications.Add(newNotification);
                        
                        break;
                    }
                case FriendOptions.AcceptOrDecline:
                    {
                        var notifications = _playerContext.MenuNotifications.Where(x => x.ToId == userId && x.MenuNotificationType == MenuNotificationType.FriendInvite).ToList();
                        var correctNotification = new MenuNotification();
                        var correctContext = new FriendContext();
                        foreach(var n in notifications)
                        {
                            FriendContext friendContext = JsonConvert.DeserializeObject<FriendContext>(n.ExtraProperties);
                            if(context.CallerUserId == friendContext.CallerUserId)
                            {
                                correctNotification = n;
                                correctContext = friendContext;
                                break;
                            }
                        }

                        var dbNotification = _playerContext.MenuNotifications.First(x => x.Id == correctNotification.Id);

                        //Si accepted, ajoute les amis 
                        if (context.Accepted)
                        {
                            // Add new FriendPair
                            _playerContext.FriendPairs.Add(new FriendPair()
                            {
                                Id = Guid.NewGuid(),
                                Friend1 = correctContext.CallerUserId,
                                Friend2 = correctContext.TargetId,
                            });
                        }
                        _playerContext.MenuNotifications.Remove(dbNotification);
                        break;
                    }
                case FriendOptions.Remove:
                    {
                        var targetUser = _playerContext.Users.First(x => x.Username == context.TargetName);
                        var friendPair = _playerContext.FriendPairs.First(x => (x.Friend1 == userId && x.Friend2 == targetUser.Id)
                        || (x.Friend2 == userId && x.Friend1 == targetUser.Id));
                        _playerContext.FriendPairs.Remove(friendPair);
                        break;
                    }
            }
            _playerContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("GetMainMenuState")]
        public async Task<ActionResult> GetMainMenuState(Guid userId)
        {
            var t = _mainMenuRepository.GetMainMenuState(userId);
            return Ok();
        }


        [HttpPut]
        [Route("CreateNewGame")]
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
                Profession = Enums.RoleType.Commander,
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
                Profession = Enums.RoleType.Commander,
                X = 0,
                Y = 0,
                Z = 0,

            };
            _playerContext.Players.Add(fredPlayerModel);
            _playerContext.Players.Add(benPlayerModel);

            //Stations
            Station station = new Station()
            {
                Id = Guid.NewGuid(),
                Name = "CookStation1", // pour faire la diff ds unity, va avoir besoin de template hehelol
                GameTaskCode = GameTaskCode.Cook,
                GameId = defaultGameGuid,
                SerializedProperties = JsonConvert.SerializeObject(new CookStationProperties()
                {
                    MoneyMade = 5,
                    State = Enums.State.Pristine
                }),
            };
            _playerContext.Stations.Add(station);

            //Items 
            Guid Item1 = Guid.NewGuid();
            _playerContext.Items.Add(new Item()
            {
                Id = Item1,
                ItemType = ItemType.Hose,
                OwnerId = defaultPlayer1Guid,
            });

            _playerContext.SaveChanges();
            return Ok();
        }
    }
}