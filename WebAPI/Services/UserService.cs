using WebAPI.Entities;
using WebAPI.Models.Requests;
using WebAPI.Repositories;

namespace WebAPI.Services;

public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository repository)
    {
        _userRepository = repository;
    }

    public async Task<(bool IsIssued, User? UserModel)> CanIssueTokenToUser(LoginRequest request)
    {
        User? user = await _userRepository.GetUserByVerifyingCredentialsOrNull(request);
        (bool IsIssued, User? UserModel) isIssuedUser = new(user is not null, user);
        return isIssuedUser;
    }
}
