using Microsoft.AspNetCore.Mvc;
using WebAPI.Interfaces;
using Shared_Resources.Models;
using Shared_Resources.GameTasks;
using Shared_Resources.Scratches;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TheCrewController : ControllerBase
    {
        private readonly PlayerContext _playerContext;
        private readonly IPlayerRepository _playerRepository;
        private readonly IGameStateRepository _gameStateRepository;
        private readonly IMainMenuRepository _mainMenuRepository;
        private readonly IPlayerService _playerService;
        private readonly IGameTaskService _gameTaskService;

        public TheCrewController(PlayerContext playerContext,
            IPlayerRepository playerRepository,
            IGameStateRepository gameStateRepository,
            IMainMenuRepository mainMenuRepository,
            IPlayerService playerService,
            IGameTaskService gameTaskService)
        {
            _playerRepository = playerRepository;
            _playerContext = playerContext;
            _gameStateRepository = gameStateRepository;
            _mainMenuRepository = mainMenuRepository;
            _playerService = playerService;
            _gameTaskService = gameTaskService;
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
        public async Task<ActionResult<ClientCallResult>> GameTask(Guid playerId, GameTaskCodes taskCode, [FromBody] List<Tuple<string, string>> parameters)
        {
            ClientCallResult result = await _gameTaskService.ExecuteGameTask(playerId, taskCode, new TaskParameters(parameters));
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
        public async Task<ActionResult<ClientCallResult>> TransferItem(Guid targetId, Guid itemId, Guid gameId) // pourrait devenir une method dans le service
        {
            await _playerService.TransferItem(targetId, itemId, gameId);
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
            //var t = _mainMenuRepository.GetMainMenuState(userId);
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