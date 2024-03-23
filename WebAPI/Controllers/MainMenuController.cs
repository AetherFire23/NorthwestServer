using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI.Entities;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.Services;
namespace WebAPI.Controllers;

[ApiController]
[Route("mainmenu")]
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
        MainMenuState mainMenuState = await _mainMenuRepository.GetMainMenuState(userId);

        try
        {
            JsonSerializerSettings setts = new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            // Serialize the object
            string serializedJson = JsonConvert.SerializeObject(mainMenuState, Formatting.None, setts);

            // Deserialize the object
            MainMenuState? deserializedObject = JsonConvert.DeserializeObject<MainMenuState>(serializedJson, setts);
        }
        catch (Exception ex)
        {
            // Handle the exception
            Console.WriteLine("An error occurred: " + ex.Message);
        }

        return Ok(mainMenuState);
    }

    [HttpPost]
    [Route("createlobby")]
    public async Task<ActionResult<Lobby>> CreateLobby([FromQuery] Guid userId)
    {
        Lobby lobby = await _lobbyService.CreateAndJoinLobby(userId);
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