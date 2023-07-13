using Azure.Core;
using Shared_Resources.DTOs;
using Shared_Resources.Models;
using Shared_Resources.Models.Requests;
using WebAPI.Services;

namespace WebAPI.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenManager _jwtTokenManager;
        public AuthenticationService(IUserService userService, IJwtTokenManager jwtTokenManager)
        {
            _userService = userService;
            _jwtTokenManager = jwtTokenManager;
        }

        public async Task<ClientCallResult> TryLogin(LoginRequest loginRequest)
        {
            (bool Allowed, UserDto UserModel) canIssueUser = await _userService.AllowIssueTokenToUser(loginRequest);

            if (canIssueUser.Allowed)
            {
                string token = _jwtTokenManager.GenerateToken(canIssueUser.UserModel);
                var result = new ClientCallResult()
                {
                    IsSuccessful = true,
                    Content = token,
                    Message = "Token issued !"
                };
                return result;
            }

            var unsuccessfulRequest = new ClientCallResult()
            {
                IsSuccessful = false,
                Content = string.Empty,
                Message = "User could not be authenticated"// for unity-sude
            };

            return unsuccessfulRequest;
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
                    Content = allowedUser.Model,
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
}
