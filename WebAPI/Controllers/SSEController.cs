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
using WebAPI.Dummies;
using WebAPI.Services;
using WebAPI.Utils;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route(SSEEndpoints.ServerSideEvents)]
    public class SSEController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenManager _tokenManager;
        private readonly ISSEClientManager _sseManager;

        public SSEController(IUserService userService, IJwtTokenManager tokenManager, ISSEClientManager sseManager)
        {
            _userService = userService;
            _tokenManager = tokenManager;
            _sseManager = sseManager;
        }

        [HttpGet]
        [Route(SSEEndpoints.EventStream)]
        public async Task GetEventStream2(Guid playerId, Guid gameId) // should activate only when logged-in so I should finish login
        {
            PlayerStruct playerStruct = new PlayerStruct(playerId, gameId);
            Console.WriteLine("req received from client");
            Response.Headers.Add("Content-Type", "text/event-stream");
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");

            try
            {
                await _sseManager.Subscribe(playerStruct, Response);
                await Task.Delay(Timeout.Infinite, Response.HttpContext.RequestAborted);



                //while (!cancellationToken.IsCancellationRequested)
                //{
                //    await Task.Delay(200, cancellationToken);

                //    SSEEventType eventType = (SSEEventType)RandomHelper.random.Next(0, 3);
                //    var dumm = new SSEData<List<Player>>(eventType, new List<Player> 
                //    { 
                //        DummyValues.Fred,
                //        new Player() 
                //        { 
                //            Id = Guid.NewGuid(),
                //            X = RandomHelper.random.Next(0, 10),
                //        } 
                //    });
                //    string line = dumm.ConvertToReadableLine();
                //    await Response.WriteAsync(line);
                //    await Response.Body.FlushAsync();
                //    Console.WriteLine("Did you receive my event?");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
            }
            finally
            {
                await _sseManager.Unsubscribe(playerStruct);
            }
        }
    }
}