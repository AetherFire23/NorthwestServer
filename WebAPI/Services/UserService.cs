
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Models.Requests;
using WebAPI.Repository.Users;

namespace WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository repository)
        {
            _userRepository = repository;
        }

        public async Task<(bool Allowed, UserDto UserModel)> AllowIssueTokenToUser(LoginRequest request)
        {
            User? user = await _userRepository.GetUserByVerifyingCredentialsOrNull(request);

            if (user is null)
            {
                return (false, null);
            }

            UserDto? userDto = await _userRepository.GetUserDtoById(user.Id);
            return (true, userDto);
        }

       // public async Task<(bool isCreated, UserDto)>
    }
}
