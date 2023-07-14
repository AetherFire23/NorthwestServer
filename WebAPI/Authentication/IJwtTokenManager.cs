using Shared_Resources.DTOs;

namespace WebAPI.Authentication;

public interface IJwtTokenManager
{
    Task<string> GenerateToken(UserDto userDto);
}