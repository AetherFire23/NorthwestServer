
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Models.Requests;

namespace WebAPI.Services;

public interface IUserService
{
    Task<(bool IsCreated, UserDto UserModel)> AllowCreateUser(RegisterRequest request);
    Task<(bool IsIssued, User? UserModel)> AllowIssueTokenToUser(LoginRequest request);
}
