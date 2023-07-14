using System;

namespace Shared_Resources.Models;

public class LoginResult
{
    public bool IsSuccessful { get; set; } = false;
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; } = Guid.Empty;
}
