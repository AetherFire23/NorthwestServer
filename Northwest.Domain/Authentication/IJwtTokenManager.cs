using Northwest.Domain.Dtos;

namespace Northwest.Domain.Authentication;

public interface IJwtTokenManager
{
    Task<string> GenerateToken(UserDto userDto);
}