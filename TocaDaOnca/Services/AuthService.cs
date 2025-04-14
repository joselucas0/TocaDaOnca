using Microsoft.EntityFrameworkCore;
using TocaDaOnca.AppDbContext;
using TocaDaOnca.Models;

namespace TocaDaOnca.Services
{
    public class AuthService
    {
        private readonly Context _context;

        public AuthService(Context context)
        {
            _context = context;
        }

        public async Task<User?> ValidateUserCredentials(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return null;

            // Verifique se a senha está em hash (implementaremos isso depois)
            // Por enquanto, vamos comparar diretamente
            bool verified = BCrypt.Net.BCrypt.Verify(password, user.Password);
            
            return verified ? user : null;
        }
    }
}