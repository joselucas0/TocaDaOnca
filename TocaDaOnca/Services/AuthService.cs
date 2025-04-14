using Microsoft.EntityFrameworkCore;
using TocaDaOnca.AppDbContext;
using TocaDaOnca.Models;

namespace TocaDaOnca.Services
{
    public class AuthService
    {
        private readonly Context _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(Context context, ILogger<AuthService> logger)
        {
            _context = context;
            _logger = logger;
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

        public async Task<Employee?> ValidateEmployeeCredentials(string email, string password)
        {
            try
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

                if (employee == null)
                {
                    _logger.LogInformation("Login attempt with non-existent employee email: {Email}", email);
                    return null;
                }

                bool verified = BCrypt.Net.BCrypt.Verify(password, employee.Password);

                if (!verified)
                {
                    _logger.LogWarning("Failed login attempt for employee email: {Email}", email);
                    return null;
                }

                _logger.LogInformation("Successful login for employee: {Email}, Manager: {IsManager}",
                    email, employee.Manager);

                return employee;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating employee credentials for email: {Email}", email);
                return null;
            }
        }
    }
}