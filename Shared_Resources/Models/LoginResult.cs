using System;

namespace Shared_Resources.Models;

public class LoginResult
{
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; } = Guid.Empty;
}
