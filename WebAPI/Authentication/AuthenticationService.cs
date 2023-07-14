using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Models;
using Shared_Resources.Models.Requests;
using WebAPI.Repository.Users;
using WebAPI.Services;

namespace WebAPI.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserService _userService;
    private readonly IJwtTokenManager _jwtTokenManager;
    private readonly IUserRepository _userRepository;
    public AuthenticationService(IUserService userService,
        IJwtTokenManager jwtTokenManager,
        IUserRepository userRepository)
    {
        _userService = userService;
        _jwtTokenManager = jwtTokenManager;
        _userRepository = userRepository;
    }

    public async Task<ClientCallResult> TryLogin(LoginRequest loginRequest)
    {
        (bool IsIssued, User? UserModel) canIssueToken = await _userService.AllowIssueTokenToUser(loginRequest);

        if (!canIssueToken.IsIssued)
        {
            var unsuccessfulRequest = new ClientCallResult
            {
                IsSuccessful = false,
                Message = "User could not be authenticated"
            };

            return unsuccessfulRequest;
        }

        UserDto userDto = await _userRepository.MapUserDtoById(canIssueToken.UserModel.Id);
        string token = await _jwtTokenManager.GenerateToken(userDto);
        var result = new ClientCallResult
        {
            IsSuccessful = true,
            Message = "Token issued !",
            Content = new LoginResult
            {
                IsSuccessful = true,
                Token = token,
                UserId = userDto.Id,
            },
        };
        return result;
    }

    /// <summary> Content is userdto</summary>
    public async Task<ClientCallResult> TryRegister(RegisterRequest registerRequest)
    {
        (bool IsCreated, UserDto Model) allowedUser = await _userService.AllowCreateUser(registerRequest);

        if (allowedUser.IsCreated)
        {
            var result = new ClientCallResult()
            {
                IsSuccessful = true,
                Content = allowedUser.Model
            };
            return result;
        }

        return new ClientCallResult()
        {
            IsSuccessful = false,
            Message = "user authentication failed !"
        };
    }
}
