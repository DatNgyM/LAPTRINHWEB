using System.Threading.Tasks;
using LAPTRINHWEB.Models;

namespace LAPTRINHWEB.Services
{
    public interface IAuthService
    {
        Task<User> LoginAsync(string email, string password);
        Task<bool> RegisterAsync(User user);
        Task<bool> EmailExistsAsync(string email);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
        Task UpdateLastLoginAsync(int userId);
    }
}