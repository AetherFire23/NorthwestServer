namespace WebAPI.Models.Requests;

public class LoginRequest
{
    public string UserName { get; set; } = string.Empty;
    public string PasswordAttempt { get; set; } = string.Empty;
}
