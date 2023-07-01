using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared_Resources.Constants.Endpoints;
using Shared_Resources.DTOs;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using Shared_Resources.Models.Requests;
using System.Text;
using WebAPI.Authentication;
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
        [Route("stream1")]
        public async Task GetEventStream()
        {
            Console.WriteLine("Demand received from client");
            Response.Headers.Add("Content-Type", "text/event-stream");
            Response.Headers.Add("Connection", "keep-alive");
            var s = Response.BodyWriter.AsStream();
            try
            {
                var cancellationToken = Response.HttpContext.RequestAborted;
                bool mustCancelRequest = cancellationToken.IsCancellationRequested;
                while (!mustCancelRequest)
                {
                    await Task.Delay(400, cancellationToken);

                    var messBytes = ConvertToBytes("0");
                    //await Response.Body.WriteAsync(messBytes, 0, messBytes.Length);

                    //await Response.WriteAsync("sex");

                   
                    await s.WriteAsync(messBytes);
                    await s.FlushAsync();

                    Console.WriteLine("DId you receive my event?");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
            }
            await Response.Body.FlushAsync();

            
            byte[] ConvertToBytes(string data)
            {
                var bytes = ASCIIEncoding.ASCII.GetBytes(data);
                return bytes;
            }
        }

        [HttpGet]
        [Route("stream2")]
        public async Task GetEventStream2()
        {
            Console.WriteLine("Demand received from client");
            Response.Headers.Add("Content-Type", "text/event-stream");
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");
            try
            {
                var cancellationToken = Response.HttpContext.RequestAborted;
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, cancellationToken);
                    await Response.WriteAsync("my line \n");
                    await Response.Body.FlushAsync();
                    Console.WriteLine("DId you receive my event?");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
            }
        }
    }
}