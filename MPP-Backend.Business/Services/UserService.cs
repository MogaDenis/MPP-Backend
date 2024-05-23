using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;
using MPP_Backend.Business.DTOs;
using MPP_Backend.Business.Services.Interfaces;
using MPP_Backend.Data.Models;
using MPP_Backend.Data.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MPP_Backend.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper) 
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserDTO?> AddUserAsync(UserForAddUpdateDTO user)
        {
            int newUserId = await _userRepository.AddUserAsync(_mapper.Map<User>(user));
            if (newUserId < 0)
            {
                throw new ValidationException("There already exists an user with the given email.");
            }

            var newUser = _mapper.Map<UserDTO>(user);
            newUser.Id = newUserId;

            return newUser;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            if (userId < 0) 
            {
                throw new ValidationException("Invalid user id.");
            }

            return await _userRepository.DeleteUserAsync(userId);
        }

        public async Task<bool> UpdateUserAsync(int userId, UserForAddUpdateDTO newUser)
        {
            if (userId < 0)
            {
                throw new ValidationException("Invalid user id.");
            }

            return await _userRepository.UpdateUserAsync(userId, _mapper.Map<User>(newUser));
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

        public async Task<User?> GetUserById(int userId)
        {
            if (userId < 0)
            {
                throw new ValidationException("Invalid user id.");
            }

            return await _userRepository.GetUserById(userId);
        }

        public async Task<User?> GetUserByEmail(string userEmail)
        {
            return await _userRepository.GetUserByEmail(userEmail);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<IEnumerable<User>> FilterUsersByRole(UserRole userRole)
        {
            return await _userRepository.FilterUsersByRole(userRole);
        }
    }
}
