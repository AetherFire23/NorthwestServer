using Microsoft.AspNetCore.Mvc;
using Shared_Resources.Constants.Endpoints;
using Shared_Resources.GameTasks;
using Shared_Resources.Models;
using WebAPI.Repositories;
using WebAPI.Services;

namespace WebAPI.Controllers;

[ApiController]
[Route(GameEndpoints.GameController)]
public class GameController : ControllerBase
{
    private readonly GameStateRepository _gameStateRepository;
    private readonly PlayerService _playerService;
    private readonly GameTaskService _gameTaskService;

    public GameController(GameStateRepository gameStateRepository,
        PlayerService playerService,
        GameTaskService gameTaskService)
    {
        _gameStateRepository = gameStateRepository;
        _playerService = playerService;
        _gameTaskService = gameTaskService;
    }

    [HttpGet]
    [Route(GameEndpoints.GameState)]
    public async Task<ActionResult<GameState>> GetGameState(Guid playerId, DateTime? lastTimeStamp)
    {
        GameState gameStateResult = await _gameStateRepository.GetPlayerGameStateAsync(playerId, lastTimeStamp);
        return Ok(gameStateResult);
    }

    [HttpPut]
    [Route(GameEndpoints.ExecuteGameTask)]
    public async Task<ActionResult> GameTask(Guid playerId, GameTaskCodes taskCode, [FromBody] List<Tuple<string, string>> parameters)
    {
        await _gameTaskService.ExecuteGameTask(playerId, taskCode, new TaskParameters(parameters));
        return Ok();
    }

    [HttpPut] // put = update, post = creation
    [Route(GameEndpoints.UpdatePlayerPosition)]
    public async Task<ActionResult> UpdatePositionByPlayerModel(Guid playerId, float x, float y) // va dependre de comment je manage les data
    {
        await _playerService.UpdatePositionAsync(playerId, x, y);
        return Ok();
    }

    [HttpPut]
    [Route(GameEndpoints.TransferItem)] // me sers meme pas du ownerId        // Hey, je veux que x owner own, voici le owner que jai. Mais si le owner que j<ai != le owner de litem ca doit larreter et refresher
    public async Task<ActionResult> TransferItem(Guid targetId, Guid ownerId, Guid itemId, Guid gameId) // pourrait devenir une method dans le service
    {
        await _playerService.TransferItem(targetId, ownerId, itemId, gameId);
        return Ok();
    }

    [HttpPut]
    [Route(GameEndpoints.ChangeRoom)]
    public async Task<ActionResult> ChangeRoom(Guid playerId, string targetRoomName)
    {
        await _playerService.ChangeRoomAsync(playerId, targetRoomName);
        return Ok();
    }
}