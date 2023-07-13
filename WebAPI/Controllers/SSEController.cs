using Microsoft.AspNetCore.Mvc;
using Shared_Resources.Constants.Endpoints;
using WebAPI.Authentication;
using WebAPI.Extensions;
using WebAPI.Repository.Users;
using WebAPI.Services;
using WebAPI.SSE;
using WebAPI.SSE.ClientManagers;
using WebAPI.SSE.SSECore;
using WebAPI.SSE.Subscribers;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route(SSEEndpoints.ServerSideEvents)]
    public class SSEController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserRepository _userRepository;

        public SSEController(
            IServiceProvider serviceProvider,
            IUserRepository userRepository)
        {
            _serviceProvider = serviceProvider;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route(SSEEndpoints.EventStream)]
        public async Task GetGameEventStream(Guid playerId, Guid gameId)
        {
            PlayerSubscriber playerSubscriber = new PlayerSubscriber(playerId, gameId);
            GameSSEClientManager sseGameManager = _serviceProvider.GetSSEManager<GameSSEClientManager>();

            await sseGameManager.InitializeAndSubscribe(playerSubscriber, Response);
        }

        [HttpGet]
        [Route(SSEEndpoints.MainMenuStream)]
        public async Task GetMainMenuEventStream(Guid userId) // should activate only when logged-in so I should finish login
        {
            // User user = await _userRepository.GetUserById(userId) ?? throw new Exception("User not found.");
            SSESubscriber subscriber = new SSESubscriber(userId);
            MainMenuClientManager sseMainMenuClientManager = _serviceProvider.GetSSEManager<MainMenuClientManager>();

            await sseMainMenuClientManager.InitializeAndSubscribe(subscriber, Response);
        }
    }
}