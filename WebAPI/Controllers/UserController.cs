using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwest.Domain.Authentication;
using Northwest.Domain.Dtos;
using Northwest.Domain.Models;
using Northwest.Domain.Models.Requests;
using Northwest.Domain.Services;
using Northwest.Persistence.Enums;
namespace Northwest.WebApi.Controllers;

[ApiController]
[Route("Users")]
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
    [Route("login")]
    [ProducesResponseType(200, Type = typeof(LoginResult))]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        await Task.Delay(250);
        LoginResult loginResult = await _authenticationService.TryLogin(request);
        return Ok(loginResult);
    }

    [HttpPost("register")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request)
    {
        UserDto userDto = await _authenticationService.TryRegister(request);
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