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
            public MainMenuController(IPlayerRepository playerRepository,
                PlayerContext playerContext,
                IChatService chatService)
            {
                _playerRepository = playerRepository;
                _playerContext = playerContext;
                _chatService = chatService;
            }

            [HttpGet]
            [Route(MainMenuEndpoints.State)]
            public async Task<ActionResult<ClientCallResult>> GetMainMenuState() 
            {
                return Ok(new MainMenuState());
            }
        }
    }
}
