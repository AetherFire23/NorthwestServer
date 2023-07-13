using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Models.Requests;

namespace WebAPI.Repository.Users
{
    public interface IUserRepository
    {
        Task<UserDto> CreateUser(RegisterRequest request);
        Task<List<GameDto>> GetActiveGameDtosForUser(Guid userId);
        Task<List<LobbyDto>> GetLobbyDtosForUser(Guid userId);
        Task<User?> GetUserById(Guid id);
        Task<User?> GetUserByVerifyingCredentialsOrNull(LoginRequest loginRequest);
        Task<UserDto> MapUserDtoById(Guid id);
        Task<bool> IsUserExists(string userName, string email);
        Task<List<UserDto>> GetUserDtosById(List<Guid> userIds);
        Task<List<UserDto>> GetUserDtosFromUser(List<User> user);
    }
}
