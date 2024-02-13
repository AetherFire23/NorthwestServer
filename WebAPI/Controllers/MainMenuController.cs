using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared_Resources.Constants.Endpoints;
using Shared_Resources.Entities;
using Shared_Resources.Models;
using WebAPI.Repositories;
using WebAPI.Services;
namespace WebAPI.Controllers;

[ApiController]
[Route(MainMenuEndpoints.MainMenu)]
public class MainMenuController : ControllerBase
{
    private readonly MainMenuRepository _mainMenuRepository;
    private readonly LobbyService _lobbyService;
    public MainMenuController(MainMenuRepository mainMenuRepository, LobbyService lobbyService)
    {
        _mainMenuRepository = mainMenuRepository;
        _lobbyService = lobbyService;
    }

    [HttpGet]
    [Route("GetMainMenuState")]
    [ProducesResponseType(200, Type = typeof(MainMenuState))]
    public async Task<ActionResult<MainMenuState>> GetMainMenuState(Guid userId)
    {
        var mainMenuState = await _mainMenuRepository.GetMainMenuState(userId);

        try
        {
            var setts = new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            // Serialize the object
            string serializedJson = JsonConvert.SerializeObject(mainMenuState, Formatting.None, setts);

            // Deserialize the object
            MainMenuState deserializedObject = JsonConvert.DeserializeObject<MainMenuState>(serializedJson, setts);
        }
        catch (Exception ex)
        {
            // Handle the exception
            Console.WriteLine("An error occurred: " + ex.Message);
        }

        return Ok(mainMenuState);
    }

    [HttpPost]
    [Route(MainMenuEndpoints.CreateLobby)]
    public async Task<ActionResult<Lobby>> CreateLobby([FromQuery] Guid userId)
    {
        var lobby = await _lobbyService.CreateAndJoinLobby(userId);
        return Ok(lobby);
    }

    [HttpPost]
    [Route("joinLobby")]
    public async Task<ActionResult> JoinLobby([FromBody] JoinLobbyRequest joinLobbyRequest)
    {
        await _lobbyService.JoinLobby(joinLobbyRequest);
        return Ok();
    }

    [HttpPost]
    [Route("StartGame")]
    public async Task<ActionResult> StartGame([FromQuery] Guid lobbyId)
    {
        await _lobbyService.StartGame(lobbyId);
        return Ok();
    }
}