﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Northwest.Domain.GameStart;
using Northwest.Domain.Models;
using Northwest.Domain.Repositories;
using Northwest.Persistence.Entities;
namespace Northwest.WebApi.Controllers;

[ApiController]
[Route("mainmenu")]
public class MainMenuController(MainMenuRepository _mainMenuRepository, LobbyService _lobbyService) : ControllerBase
{

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
            var deserializedObject = JsonConvert.DeserializeObject<MainMenuState>(serializedJson, setts);
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