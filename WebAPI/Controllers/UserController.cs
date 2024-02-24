using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared_Resources.Constants.Endpoints;
using Shared_Resources.DTOs;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using Shared_Resources.Models.Requests;
using WebAPI.Authentication;
using WebAPI.Services;
namespace WebAPI.Controllers;

[ApiController]
[Route(UserEndpoints.Users)]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly AuthenticationService _authenticationService;

    public UserController(UserService userService, AuthenticationService authenticationService)
    {
        _userService = userService;
        _authenticationService = authenticationService;
    }

    [HttpPost]
    [Route(UserEndpoints.Login)]
    [ProducesResponseType(200, Type = typeof(LoginResult))]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        await Task.Delay(250);
        var loginResult = await _authenticationService.TryLogin(request);
        return Ok(loginResult);
    }

    [HttpPost(UserEndpoints.Register)]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request)
    {
        var userDto = await _authenticationService.TryRegister(request);
        return Ok(userDto);
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