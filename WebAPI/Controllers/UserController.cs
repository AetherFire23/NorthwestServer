using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared_Resources.Constants.Endpoints;
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
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authenticationService;

    public UserController(IUserService userService,
        IAuthenticationService authenticationService)
    {
        _userService = userService;
        _authenticationService = authenticationService;
    }

    [HttpPost]
    [Route(UserEndpoints.Login)]
    public async Task<ActionResult<ClientCallResult>> Login([FromBody] LoginRequest request)
    {
        var clientCallResult = await _authenticationService.TryLogin(request);
        return Ok(clientCallResult);
    }

    //   just seed that shit first
    [HttpPost(UserEndpoints.Register)]
    public async Task<ActionResult<ClientCallResult>> Register([FromBody] RegisterRequest request)
    {
        ClientCallResult clientCallResult = await _authenticationService.TryRegister(request);
        return Ok(clientCallResult);
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