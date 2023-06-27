using Shared_Resources.DTOs;

namespace WebAPI.Authentication
{
    public class TokenUser
    {
        public UserDto User { get; set; } = new UserDto();
        public string Token { get; set; } = string.Empty;
    }
}
