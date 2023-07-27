using Microsoft.AspNetCore.Mvc;
using Shared_Resources.Constants.Endpoints;
using Shared_Resources.Constants.Mapper;
using Shared_Resources.GameTasks;
using Shared_Resources.Models;
using Shared_Resources.Scratches;
using WebAPI.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
[Route(GameEndpoints.GameController)]
public class GameController : ControllerBase
{
    private readonly PlayerContext _playerContext;
    private readonly IPlayerRepository _playerRepository;
    private readonly IGameStateRepository _gameStateRepository;
    private readonly IPlayerService _playerService;
    private readonly IGameTaskService _gameTaskService;

    public GameController(PlayerContext playerContext,
        IPlayerRepository playerRepository,
        IGameStateRepository gameStateRepository,
        IMainMenuRepository mainMenuRepository,
        IPlayerService playerService,
        IGameTaskService gameTaskService)
    {
        _playerRepository = playerRepository;
        _playerContext = playerContext;
        _gameStateRepository = gameStateRepository;
        _playerService = playerService;
        _gameTaskService = gameTaskService;
    }

    [HttpGet]
    [Route(GameEndpoints.GameState)]
    public async Task<ActionResult<ClientCallResult>> GetGameState(Guid playerId, DateTime? lastTimeStamp)
    {
        ClientCallResult gameStateResult = await _gameStateRepository.GetPlayerGameStateAsync(playerId, lastTimeStamp);
        return Ok(gameStateResult);
    }

    [HttpPut]
    [Route(GameEndpoints.ExecuteGameTask)]
    public async Task<ActionResult<ClientCallResult>> GameTask(Guid playerId, GameTaskCodes taskCode, [FromBody] List<Tuple<string, string>> parameters)
    {
        ClientCallResult result = await _gameTaskService.ExecuteGameTask(playerId, taskCode, new TaskParameters(parameters));
        return Ok(result);
    }

    [HttpPut] // put = update, post = creation
    [Route(GameEndpoints.UpdatePlayerPosition)]
    public async Task<ActionResult<ClientCallResult>> UpdatePositionByPlayerModel(Guid playerId, float x, float y) // va dependre de comment je manage les data
    {
        await _playerService.UpdatePositionAsync(playerId, x, y);
        return Ok();
    }

    [HttpPut]
    [Route(GameEndpoints.TransferItem)] // me sers meme pas du ownerId        // Hey, je veux que x owner own, voici le owner que jai. Mais si le owner que j<ai != le owner de litem ca doit larreter et refresher
    public async Task<ActionResult<ClientCallResult>> TransferItem(Guid targetId, Guid ownerId, Guid itemId, Guid gameId) // pourrait devenir une method dans le service
    {
        await _playerService.TransferItem(targetId, ownerId, itemId, gameId);
        return Ok();
    }

    [HttpPut]
    [Route(GameEndpoints.ChangeRoom)]
    public async Task<ActionResult<ClientCallResult>> ChangeRoom(Guid playerId, string targetRoomName)
    {
        var result = await _playerService.ChangeRoomAsync(playerId, targetRoomName);
        return Ok(result);
    }
}