
using Microsoft.EntityFrameworkCore;
using LAPTRINHWEB.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System;

namespace LAPTRINHWEB.Services
{
    public class AuthService : IAuthService
    {
        private readonly TourDbContext _context;

        public AuthService(TourDbContext context)
        {
            _context = context;
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email && u.Is_Active);

            if (user != null && VerifyPassword(password, user.Password_Hash))
            {
                await UpdateLastLoginAsync(user.ID_User);
                return user;
            }

            return null;
        }

        public async Task<bool> RegisterAsync(User user)
        {
            if (await EmailExistsAsync(user.Email))
                return false;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "EasyTrips_Salt"));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.Last_Login = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}