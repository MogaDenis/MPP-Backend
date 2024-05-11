using MPP_Backend.Data.Models;

namespace MPP_Backend.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> CreateUserAsync(User user);
        Task<bool> CheckUserLoginRequest(User user);
    }
}
