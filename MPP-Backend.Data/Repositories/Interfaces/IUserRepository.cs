using MPP_Backend.Data.Models;

namespace MPP_Backend.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmail(string userEmail);
        Task<User?> GetUserById(int userId);
        Task<IEnumerable<User>> GetAllUsers();
        Task<IEnumerable<User>> FilterUsersByRole(UserRole userRole);

        Task<int> AddUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> UpdateUserAsync(int userId, User newUser);

        Task<bool> CheckUserLoginRequest(User user);


    }
}
