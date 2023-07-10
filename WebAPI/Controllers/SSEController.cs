using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared_Resources.Constants.Endpoints;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using Shared_Resources.Models.Requests;
using Shared_Resources.Models.SSE;
using System.Text;
using WebAPI.Authentication;
using WebAPI.Constants;
using WebAPI.Dummies;
using WebAPI.Repository.Users;
using WebAPI.Services;
using WebAPI.SSE;
using WebAPI.Utils;
using WebAPI.Utils.Extensions;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route(SSEEndpoints.ServerSideEvents)]
    public class SSEController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenManager _tokenManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPlayerRepository _playerRepository;
        private readonly IUserRepository _userRepository;

        public SSEController(IUserService userService,
            IJwtTokenManager tokenManager,
            IServiceProvider serviceProvider,
            IPlayerRepository playerRepository,
            IUserRepository userRepository)
        {
            _userService = userService;
            _tokenManager = tokenManager;
            _serviceProvider = serviceProvider;
            _playerRepository = playerRepository;
            _userRepository = userRepository;
        }

        // jpourrais envoyer en fait un SSESubscriptionOption
        // genre IsMainMenu
        // SubscrinerId
        // GameId (if applicable)
        [HttpGet]
        [Route(SSEEndpoints.EventStream)]
        public async Task GetEventStream2(Guid playerId, Guid gameId) // should activate only when logged-in so I should finish login
        {
            PlayerStruct playerStruct = new PlayerStruct(playerId, gameId);
            GameSSEClientManager sseGameManager = _serviceProvider.GetSSEManager<GameSSEClientManager>();

            await sseGameManager.InitializeAndSubscribe(playerStruct, Response);
        }

        [HttpGet]
        [Route(SSEEndpoints.MainMenuStream)]
        public async Task GetEventStreamMainMenu(Guid userId, Guid gameId) // should activate only when logged-in so I should finish login
        {
            var user = await _userRepository.GetUserById(userId) ?? throw new Exception("User not found.");
            var sseMainMenuClientManager = _serviceProvider.GetSSEManager<MainMenuClientManager>();

            await sseMainMenuClientManager.InitializeAndSubscribe(user, Response);
        }
    }
}