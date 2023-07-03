using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared_Resources.Constants.Endpoints;
using Shared_Resources.DTOs;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using Shared_Resources.Models.Requests;
using WebAPI.Authentication;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route(UserEndpoints.Users)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenManager _tokenManager;

        public UserController(IUserService userService, IJwtTokenManager tokenManager)
        {
            _userService = userService;
            _tokenManager = tokenManager;
        }

        [HttpPost]
        [Route(UserEndpoints.Login)]
        public async Task<ActionResult<ClientCallResult>> Login([FromBody] LoginRequest request)
        {
            (bool Allowed, UserDto UserModel) canIssueUser = await _userService.AllowIssueTokenToUser(request);

            if (canIssueUser.Allowed)
            {
                string token = _tokenManager.GenerateToken(canIssueUser.UserModel);
                var result = new ClientCallResult()
                {
                    IsSuccessful = true,
                    Content = token,
                    Message = "Token issued !"
                };
                return Ok(result);
            }

            var unsuccessfulRequest = new ClientCallResult()
            {
                IsSuccessful = false,
                Content = string.Empty,
                Message = "User could not be authenticated"// for unity-sude
            };

            return Ok(unsuccessfulRequest);
        }

        //   just seed that shit first
        [HttpPost(UserEndpoints.Register)]
        public async Task<ActionResult<ClientCallResult>> Register([FromBody] RegisterRequest request)
        {
            (bool IsCreated, UserDto Model) allowedUser = await _userService.AllowCreateUser(request);

            if (allowedUser.IsCreated)
            {
                var result = new ClientCallResult()
                {
                    IsSuccessful = true,
                    Content = allowedUser.Model,
                };
                return Ok(result);
            }

            
            return Ok(new ClientCallResult()
            {
                IsSuccessful = false,
                Message = "user authentication failed !"
            });
        }

        [Authorize(Roles = nameof(RoleName.PereNoel))]
        [HttpGet("test")]
        public async Task<ActionResult> Test()
        {
            Console.WriteLine("sEXYAuthorizsed2");
            await Task.Delay(1);
            return Ok();
        }
    }
}