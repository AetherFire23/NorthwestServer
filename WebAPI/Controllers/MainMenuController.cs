namespace WebAPI.Controllers
{
    using global::WebAPI.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Shared_Resources.Constants.Endpoints;
    using Shared_Resources.Models;

    namespace WebAPI.Controllers
    {
        [ApiController]
        [Route(MainMenuEndpoints.MainMenu)]
        public class MainMenuController : ControllerBase
        {
            private readonly IPlayerRepository _playerRepository;
            private readonly PlayerContext _playerContext;
            private readonly IChatService _chatService;
            private readonly IMainMenuRepository _mainMenuRepository;
            public MainMenuController(IPlayerRepository playerRepository,
                PlayerContext playerContext,
                IChatService chatService, IMainMenuRepository mainMenuRepository)
            {
                _playerRepository = playerRepository;
                _playerContext = playerContext;
                _chatService = chatService;
                _mainMenuRepository = mainMenuRepository;
            }

            [HttpGet]
            [Route(MainMenuEndpoints.State)]
            public async Task<ActionResult<ClientCallResult>> GetMainMenuState(Guid userId)
            {
                var clientCallResult = await _mainMenuRepository.GetMainMenuState(userId);
                var deserializeResult = clientCallResult.DeserializeContent<MainMenuState>();
                return Ok(clientCallResult);
            }

            [HttpPost]
            [Route(MainMenuEndpoints.CreateLobby)]
            public async Task<ActionResult<ClientCallResult>> CreateLobby()
            {
                // add lobby
                // will have to think about empty lobbies cleanup
                return Ok();
            }
        }
    }
}
