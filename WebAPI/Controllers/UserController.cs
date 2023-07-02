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
       //[HttpPost("register")]
       // public async Task<IActionResult> Register([FromBody] RegisterRequest request)
       // {
       //     var user = await _userService.CreateUser(request.Username, request.Password, request.Email).ConfigureAwait(false);
       //     if (user != null)
       //     {
       //         string token = _tokenManager.GenerateToken(user);
       //         return Ok(new { user, token });
       //     }

       //     return Unauthorized();
       // }


        [Authorize(Roles = nameof(RoleName.PereNoel))]
        [HttpGet("test")]
        public async Task<ActionResult> Test()
        {
            Console.WriteLine("sEXYAuthorizsed2");
            await Task.Delay(1);
            return Ok();
        }





        //[HttpPost("token")]
        //public async Task<IActionResult> GetUserFromToken([FromBody] TokenLoginRequest request)
        //{
        //    var validatedToken = _tokenManager.ValidateToken(request.Token);
        //    if (validatedToken != null)
        //    {
        //        string? userId = validatedToken?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //        if (userId == null) return Unauthorized();

        //        var user = await _userService.GetUserById(userId).ConfigureAwait(false);
        //        if (user != null)
        //        {
        //            return Ok(user);
        //        }
        //    }

        //    return Unauthorized();
        //}

        //[HttpPost("swaggerLogin")]
        //[Consumes("application/x-www-form-urlencoded")]
        //public async Task<IActionResult> SwaggerLogin([FromForm] LoginRequest request)
        //{
        //    var user = await _userService.GetUser(request.Username, request.Password).ConfigureAwait(false);
        //    if (user != null)
        //    {
        //        string token = _tokenManager.GenerateToken(user);

        //        return Ok(new
        //        {
        //            access_token = token,
        //            token_type = "Bearer",
        //            expires_in = 999999
        //        });
        //    }

        //    return Unauthorized();
        //}
    }
}