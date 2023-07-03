using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Models.Requests;

namespace WebAPI.Repository.Users
{
    public interface IUserRepository
    {
        Task<UserDto> CreateUser(RegisterRequest request);
        Task<User?> GetUserById(Guid id);
        Task<User?> GetUserByVerifyingCredentialsOrNull(LoginRequest loginRequest);
        Task<UserDto> GetUserDtoById(Guid id);
        Task<bool> IsUserExists(string userName, string email);
    }
}
