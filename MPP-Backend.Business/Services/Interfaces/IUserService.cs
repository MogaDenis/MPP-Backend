using Microsoft.AspNetCore.Identity.Data;
using MPP_Backend.Business.DTOs;
using MPP_Backend.Data.Models;

namespace MPP_Backend.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserById(int userId);
        Task<User?> GetUserByEmail(string userEmail);
        Task<IEnumerable<User>> GetAllUsers();
        Task<IEnumerable<User>> FilterUsersByRole(UserRole userRole);

        Task<bool> CheckUserLoginRequest(LoginRequest loginRequest);

        Task<UserDTO?> AddUserAsync(UserForAddUpdateDTO user);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> UpdateUserAsync(int userId, UserForAddUpdateDTO newUser);
    }
}
