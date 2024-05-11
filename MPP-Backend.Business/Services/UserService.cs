using Microsoft.AspNetCore.Identity.Data;
using MPP_Backend.Business.Services.Interfaces;
using MPP_Backend.Data.Models;
using MPP_Backend.Data.Repositories.Interfaces;

namespace MPP_Backend.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) 
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<bool> CreateUserAsync(RegisterRequest registerRequest)
        {
            User user = new()
            {
                Email = registerRequest.Email,
                Password = registerRequest.Password
            };

            return await _userRepository.CreateUserAsync(user);
        }

        public async Task<bool> CheckUserLoginRequest(LoginRequest loginRequest)
        {
            User user = new()
            {
                Email = loginRequest.Email,
                Password = loginRequest.Password
            };

            return await _userRepository.CheckUserLoginRequest(user);
        }
    }
}
