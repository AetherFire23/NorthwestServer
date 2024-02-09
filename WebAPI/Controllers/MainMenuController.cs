using Microsoft.AspNetCore.Mvc;
using Shared_Resources.Constants.Endpoints;
using Shared_Resources.Models;
using WebAPI.Repositories;
namespace WebAPI.Controllers;

[ApiController]
[Route(MainMenuEndpoints.MainMenu)]
public class MainMenuController : ControllerBase
{
    private readonly MainMenuRepository _mainMenuRepository;
    public MainMenuController(MainMenuRepository mainMenuRepository)
    {
        _mainMenuRepository = mainMenuRepository;
    }

    [HttpGet]
    [Route(MainMenuEndpoints.State)]
    [ProducesResponseType(200, Type = typeof(MainMenuState))]
    public async Task<ActionResult> GetMainMenuState(Guid userId)
    {
        var clientCallResult = await _mainMenuRepository.GetMainMenuState(userId);
        return Ok(clientCallResult);
    }

    [HttpPost]
    [Route(MainMenuEndpoints.CreateLobby)]
    public async Task<ActionResult> CreateLobby()
    {
        // add lobby
        // will have to think about empty lobbies cleanup
        return Ok();
    }

    [HttpPost]
    [Route("joinLobby")]
    public async Task<ActionResult> JoinLobby([FromQuery] string lobbyId)
    {
        return Ok();
    }
}

