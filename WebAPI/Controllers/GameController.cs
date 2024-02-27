using Microsoft.AspNetCore.Mvc;
using WebAPI.GameTasks;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.Services;
namespace WebAPI.Controllers;

[ApiController]
[Route("Game")]
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
    [Route("gamestate")]
    public async Task<ActionResult<GameState>> GetGameState(Guid playerId, DateTime? lastTimeStamp)
    {
        GameState gameStateResult = await _gameStateRepository.GetPlayerGameStateAsync(playerId, lastTimeStamp);
        return Ok(gameStateResult);
    }

    [HttpPut]
    [Route("executeTask")]
    public async Task<ActionResult> GameTask([FromQuery] Guid playerId, [FromQuery] GameTaskCodes taskCode, [FromBody] Dictionary<string, string> parameters)
    {
        await _gameTaskService.ExecuteGameTask(playerId, taskCode, new TaskParameters(parameters));
        return Ok();
    }

    [HttpPut] // put = update, post = creation
    [Route("updateplayerposition")]
    public async Task<ActionResult> UpdatePositionByPlayerModel(Guid playerId, float x, float y)
    {
        await _playerService.UpdatePositionAsync(playerId, x, y);
        return Ok();
    }

    [HttpPut]
    [Route("transferitem")]
    public async Task<ActionResult> TransferItem(Guid targetId, Guid ownerId, Guid itemId, Guid gameId) // pourrait devenir une method dans le service
    {
        await _playerService.TransferItem(targetId, ownerId, itemId, gameId);
        return Ok();
    }

    [HttpPut]
    [Route("changeroom")]
    public async Task<ActionResult> ChangeRoom(Guid playerId, string targetRoomName)
    {
        await _playerService.ChangeRoomAsync(playerId, targetRoomName);
        return Ok();
    }
}