
using Shared_Resources.DTOs;
using Shared_Resources.Models.Requests;

namespace WebAPI.Services
{
    public interface IUserService
    {
        Task<(bool Allowed, UserDto UserModel)> AllowIssueTokenToUser(LoginRequest request);
    }
}
