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

namespace WebAPI.Controllers
{
    [ApiController]
    [Route(SSEEndpoints.ServerSideEvents)]
    public class SSEController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenManager _tokenManager;

        public SSEController(IUserService userService, IJwtTokenManager tokenManager)
        {
            _userService = userService;
            _tokenManager = tokenManager;
        }

        [HttpGet]
        [Route(SSEEndpoints.EventStream)]
        public async Task GetEventStream2() // should activate only when logged-in so I should finish login
        {
            Console.WriteLine("req received from client");
            Response.Headers.Add("Content-Type", "text/event-stream");
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");
            try
            {
                var cancellationToken = Response.HttpContext.RequestAborted;
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, cancellationToken);

                    var dumm = new SSEData<List<Player>>(EventType.DummyEvent, new List<Player> { DummyValues.Fred, DummyValues.Ben });
                    string line = dumm.ConvertToReadableLine();
                    await Response.WriteAsync(line);
                    await Response.Body.FlushAsync();
                    Console.WriteLine("Did you receive my event?");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
            }
            finally
            {

            }
        }
    }
}