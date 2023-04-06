using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using WebAPI.Db_Models;
using WebAPI.Dummies;
using WebAPI.Entities;
using WebAPI.Enums;
using WebAPI.GameTasks;
using WebAPI.Interfaces;
using WebAPI.Models;
using WebAPI.Temp_settomgs;
using WebAPI.AutoMapper;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TheCrewController : ControllerBase
    {
        private readonly PlayerContext _playerContext;
        private readonly ILogger<TheCrewController> _logger;
        private readonly IPlayerRepository _playerRepository;
        private readonly IGameStateRepository _gameStateRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRoomRepository _roomRepository;
        private readonly IGameActionsRepository _gameActionsRepository;
        private readonly IMainMenuRepository _mainMenuRepository;
        private readonly IPlayerService _playerService;
        private readonly IGameStateService _gameStateService;
        private readonly IGameTaskService _gameTaskService;
        private readonly IFriendService _friendService;
        private readonly IGameMakerService _gameMakerService;
        private readonly IMapper _mapper;

        public TheCrewController(ILogger<TheCrewController> logger,
            PlayerContext playerContext,
            IPlayerRepository playerRepository,
            IGameStateRepository gameStateRepository,
            IServiceProvider serviceProvider,
            IRoomRepository roomRepository,
            IGameActionsRepository gameActionsRepository,
            IMainMenuRepository mainMenuRepository,
            IPlayerService playerService,
            IGameStateService gameStateService,
            IGameTaskService gameTaskService,
            IFriendService friendService,
            IGameMakerService gameMakerService,
            IMapper mapper)
        {
            _logger = logger;
            _playerRepository = playerRepository;
            _playerContext = playerContext;
            _gameStateRepository = gameStateRepository;
            _serviceProvider = serviceProvider;
            _roomRepository = roomRepository;
            _gameActionsRepository = gameActionsRepository;
            _mainMenuRepository = mainMenuRepository;
            _playerService = playerService;
            _gameStateService = gameStateService;
            _gameTaskService = gameTaskService;
            _friendService = friendService;
            _gameMakerService = gameMakerService;
            _mapper = mapper;
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
        public async Task<ActionResult<ClientCallResult>> GameTask(Guid playerId, GameTaskCode taskCode, [FromBody] Dictionary<string, string> parameters)
        {
            ClientCallResult result = _gameTaskService.ExecuteGameTask(playerId, taskCode, parameters);
            return Ok(result);
        }

        [HttpPut] // put = update, post = creation
        [Route("UpdatePositionByPlayerModel")]
        public async Task<ActionResult<ClientCallResult>> UpdatePositionByPlayerModel([FromBody] Player unityPlayerModel) // va dependre de comment je manage les data
        {
            _playerService.UpdatePosition(unityPlayerModel);
            return Ok();
        }

        [HttpPut]
        [Route("TransferItem")] // me sers meme pas du ownerId
        public async Task<ActionResult<ClientCallResult>> TransferItem(Guid ownerId, Guid targetId, Guid itemId) // pourrait devenir une method dans le service
        {
            _playerService.TransferItem(ownerId, targetId, itemId);
            return Ok();
        }


        // jva toute deleter
        [HttpPut]
        [Route("InitiateExpedition")]
        public async Task<ActionResult<ClientCallResult>> JoinExpedition(Guid playerId, string expeditionName)
        {
            var player = _playerRepository.GetPlayer(playerId);
            var expedition = _playerContext.Expeditions.First(x => x.Name == expeditionName && player.GameId == x.GameId);

            if (expedition.IsCreated)
            {
                // join as new member ?
            }

            if (expedition.IsAvailableForCreation)
            {
                // create ?

            }

            return ClientCallResult.Success;
        }

        [HttpPut]
        [Route("ChangeRoom")]
        public async Task<ActionResult<ClientCallResult>> ChangeRoom(Guid playerId, string targetRoomName)
        {
            var result = _playerService.ChangeRoom(playerId, targetRoomName);
            return Ok(result);
        }

        [HttpPut]
        [Route("GetMainMenuState")]
        public async Task<ActionResult<ClientCallResult>> GetMainMenuState(Guid userId)
        {
            var t = _mainMenuRepository.GetMainMenuState(userId);
            return Ok();
        }

        [HttpPut]
        [Route("1-CreateNewGameDummyGame")]
        public async Task<ActionResult> CreateNewGameDummyGame()
        {


            _gameMakerService.CreateDummyGame();
            return Ok();
        }

        [HttpPut]
        [Route("1-CreateNewGame")]
        public async Task<ActionResult> CreateNewGame()
        {
            _roomRepository.CreateNewRooms(DummyValues.defaultGameGuid);

            _playerContext.Games.Add(DummyValues.Game);

            var f = DummyValues.Fred;
            var b = DummyValues.Ben;

            f.CurrentGameRoomId = _playerContext.Rooms.First().Id;
            b.CurrentGameRoomId = _playerContext.Rooms.First().Id;
            _playerContext.Players.Add(f);

            _playerContext.Players.Add(b);

            _playerContext.Items.Add(DummyValues.Item);

            _playerContext.Stations.Add(DummyValues.Station);

            _playerContext.Expeditions.Add(DummyValues.Expedition1);

            _playerContext.SaveChanges();
            return Ok();
        }



        [HttpPut]
        [Route("2-AddDefaultValues")]
        public async Task<ActionResult> AddDefaultValues()
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
        [Route("3-AddSettings")]
        public async Task<ActionResult> AddSettings()
        {
            var cookSetting = new TaskSetting()
            {
                Id = Guid.NewGuid(),
                TaskCode = GameTaskCode.Cook,
                SerializedProperties = JsonConvert.SerializeObject(new CookSettingProperties())
            };

            _playerContext.TaskSettings.Add(cookSetting);

            var CleanSetting = new TaskSetting()
            {
                Id = Guid.NewGuid(),
                TaskCode = GameTaskCode.Cook,
                SerializedProperties = JsonConvert.SerializeObject(new CleanSettingProperties())
            };
            _playerContext.TaskSettings.Add(CleanSetting);

            _playerContext.SaveChanges();
            return Ok();
        }


        [HttpPut]
        [Route("Yeah")]
        public async Task<ActionResult<DestMappa>> TEstMapper()
        {
            var src = new SourceMappa()
            {
                Name = "Fred",
                Same = 3
            };
            var test = _mapper.Map<DestMappa>(src);
            return Ok(test);
        }

        [HttpPut]
        [Route("unitTestTest")]
        public async Task<ActionResult> TestXUnit()
        {
            return Ok();
        }
    }
}