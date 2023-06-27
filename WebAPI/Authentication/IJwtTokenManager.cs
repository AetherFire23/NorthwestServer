using Shared_Resources.DTOs;
using System.Security.Claims;

namespace WebAPI.Authentication
{
    public interface IJwtTokenManager
    {
        string GenerateToken(UserDto user);
        ClaimsPrincipal ValidateToken(string token);
    }
}