using Shared_Resources.Models;
using Shared_Resources.Models.Requests;

namespace WebAPI.Authentication
{
    public interface IAuthenticationService
    {
        Task<ClientCallResult> TryLogin(LoginRequest loginRequest);
        Task<ClientCallResult> TryRegister(RegisterRequest registerRequest);
    }
}
