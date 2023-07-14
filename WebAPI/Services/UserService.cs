
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Models.Requests;
using WebAPI.Repository.Users;

namespace WebAPI.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository repository)
    {
        _userRepository = repository;
    }

    public async Task<(bool IsIssued, User? UserModel)> AllowIssueTokenToUser(LoginRequest request)
    {
        User? user = await _userRepository.GetUserByVerifyingCredentialsOrNull(request);
        (bool IsIssued, User? UserModel) isIssuedUser = new(user is not null, user);
        return isIssuedUser;
    }

    public async Task<(bool IsCreated, UserDto UserModel)> AllowCreateUser(RegisterRequest request)
    {
        bool userExists = await _userRepository.IsUserExists(request.UserName, request.Email);
        if (userExists) return (false, null);

        UserDto user = await _userRepository.CreateUser(request);
        return (true, user);
    }
}
