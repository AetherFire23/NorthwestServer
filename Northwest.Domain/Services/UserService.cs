using Northwest.Domain.Models.Requests;
using Northwest.Domain.Repositories;
using Northwest.Persistence.Entities;

namespace Northwest.Domain.Services;

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
