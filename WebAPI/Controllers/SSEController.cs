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

        public SSEController(IUserService userService,
            IJwtTokenManager tokenManager,
            IServiceProvider serviceProvider)
        {
            _userService = userService;
            _tokenManager = tokenManager;
            _serviceProvider = serviceProvider;
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
            Console.WriteLine("req received from client");
            Response.Headers.Add("Content-Type", "text/event-stream");
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");
            GameSSEClientManager sseGameManager = _serviceProvider.GetSSEManager<GameSSEClientManager>();
            try
            {
                // send heartbeat to client to ensure successCode or else it just hangs 
                await Response.WriteAndFlushSSEData(Heart.Beat);
                await sseGameManager.Subscribe(playerStruct, Response);

                await Task.Delay(Timeout.Infinite, Response.HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
            }
            finally
            {
                await sseGameManager.Unsubscribe(playerStruct);
            }
        }
    }
}