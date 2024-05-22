using Microsoft.EntityFrameworkCore;
using MPP_Backend.Common.PasswordHasher;
using MPP_Backend.Data.Models;
using MPP_Backend.Data.Repositories.Interfaces;

namespace MPP_Backend.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CarManagerContext _context;

        public UserRepository(CarManagerContext context) 
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> AddUserAsync(User user)
        {
            var existentUser = await _context.Users.FirstOrDefaultAsync(currentUser => currentUser.Email == user.Email);
            if (existentUser != null)
            {
                return -1;
            }

            string hashedPassword = PasswordHasher.HashPassword(user.Password);
            user.Password = hashedPassword;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateUserAsync(int userId, User newUser)
        {
            var userToUpdate = await _context.Users.FindAsync(userId);
            if (userToUpdate == null)
            {
                return false;
            }

            if (userToUpdate.Email != newUser.Email)
            {
                var userWithNewEmail = await _context.Users
                    .FirstOrDefaultAsync(currentUser => currentUser.Email == newUser.Email);
                if (userWithNewEmail != null)
                {
                    return false;
                }
            }

            newUser.Id = userId;

            string hashedPassword = PasswordHasher.HashPassword(newUser.Password);
            newUser.Password = hashedPassword;

            _context.Users.Entry(userToUpdate).CurrentValues.SetValues(newUser);

            return true;
        }

        public async Task<bool> CheckUserLoginRequest(User user)
        {
            var userFromDatabase = await _context.Users.FirstOrDefaultAsync(currentUser => currentUser.Email == user.Email);

            if (userFromDatabase == null) 
            {
                return false;
            }

            return PasswordHasher.VerifyPassword(user.Password, userFromDatabase.Password);
        }

        public async Task<User?> GetUserByEmail(string userEmail)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email == userEmail);
        }

        public async Task<User?> GetUserById(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<User>> FilterUsersByRole(UserRole userRole)
        {
            return await _context.Users.Where(user => user.Role == userRole).ToListAsync();
        }
    }
}
