using Microsoft.AspNetCore.Identity.Data;

namespace MPP_Backend.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> CheckUserLoginRequest(LoginRequest loginRequest);
        Task<bool> CreateUserAsync(RegisterRequest registerRequest);
    }
}
