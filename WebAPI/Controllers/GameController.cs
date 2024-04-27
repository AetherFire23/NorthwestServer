using Microsoft.AspNetCore.Mvc;
using Northwest.Domain.GameTasks;
using Northwest.Domain.Models;
using Northwest.Domain.Repositories;
using Northwest.Domain.Services;

namespace Northwest.WebApi.Controllers;

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
    [Route("GetGameState")]
    public async Task<ActionResult<GameState>> GetGameState(Guid playerId, DateTime? lastTimeStamp)
    {
        var gameStateResult = await _gameStateRepository.GetPlayerGameStateAsync(playerId, lastTimeStamp);
        return Ok(gameStateResult);
    }

    [HttpPut]
    [Route("executeTask")]
    public async Task<ActionResult> GameTask([FromBody] List<List<GameTaskTargetInfo>> gameTaskTargets, [FromQuery] Guid playerId, [FromQuery] GameTaskCodes taskCode)
    {
        await _gameTaskService.ExecuteGameTask(playerId, taskCode, gameTaskTargets);
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
    public async Task<ActionResult> TransferItem([FromQuery] Guid targetId, [FromQuery] Guid itemOwnerId, [FromQuery] Guid itemId, [FromQuery] Guid gameId) // pourrait devenir une method dans le service
    {
        await _playerService.TransferItem(targetId, itemOwnerId, itemId, gameId);
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