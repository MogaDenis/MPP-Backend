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

        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
                string hashedPassword = PasswordHasher.HashPassword(user.Password);
                user.Password = hashedPassword;
            }
            catch (Exception)
            {
                return false;
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

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
    }
}
