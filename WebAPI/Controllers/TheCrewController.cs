using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using WebAPI.Dummies;
using WebAPI.GameTasks;
using WebAPI.Interfaces;
using WebAPI.Temp_settomgs;
using WebAPI.AutoMapper;
using Shared_Resources.Entities;
using Shared_Resources.Models;
using Shared_Resources.GameTasks;

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
            var gameState = await _gameStateRepository.GetPlayerGameStateAsync(playerId, lastTimeStamp);
            return Ok(gameState);
        }

        [HttpPut]
        [Route("TryExecuteGameTask")]
        public async Task<ActionResult<ClientCallResult>> GameTask(Guid playerId, GameTaskCodes taskCode, [FromBody] Dictionary<string, string> parameters)
        {
            ClientCallResult result = await _gameTaskService.ExecuteGameTask(playerId, taskCode, parameters);
            return Ok(result);
        }

        [HttpPut] // put = update, post = creation
        [Route("UpdatePositionById")]
        public async Task<ActionResult<ClientCallResult>> UpdatePositionByPlayerModel(Guid playerId, float x, float y) // va dependre de comment je manage les data
        {
            await _playerService.UpdatePositionAsync(playerId, x, y);
            return Ok();
        }

        [HttpPut]
        [Route("TransferItem")] // me sers meme pas du ownerId
        public async Task<ActionResult<ClientCallResult>> TransferItem(Guid targetId, Guid itemId) // pourrait devenir une method dans le service
        {
            await _playerService.TransferItem(targetId, itemId);
            return Ok();
        }

        [HttpPut]
        [Route("InitiateExpedition")] // should be legacy I think
        public async Task<ActionResult<ClientCallResult>> JoinExpedition(Guid playerId, string expeditionName)
        {
            var player = await _playerRepository.GetPlayerAsync(playerId);
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
            var result = await _playerService.ChangeRoomAsync(playerId, targetRoomName);
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
        [Route("CHangbenPosition")]
        public async Task<ActionResult> ChangeBen(float x, float y)
        {
            var player = await _playerRepository.GetPlayerAsync(new Guid("B3543B2E-CD81-479F-B99E-D11A8AAB37A0"));

            player.X = x;
            player.Y = y;
            await _playerContext.SaveChangesAsync();
            return Ok();
        }
    }
}