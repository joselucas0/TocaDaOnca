using Microsoft.AspNetCore.Mvc;
using TocaDaOnca.Models;
using TocaDaOnca.Services;

namespace TocaDaOnca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly TokenService _tokenService;

        public AuthController(AuthService authService, TokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("user")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _authService.ValidateUserCredentials(model.Email, model.Password);

            if (user == null)
                return Unauthorized(new { message = "Email ou senha incorretos" });

            var token = _tokenService.GenerateToken(user);

            return Ok(new
            {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                premium = user.Premium,
                token = token
            });
        }

        [HttpPost("staff")]
        public async Task<IActionResult> StaffLogin([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Dados de login inválidos", errors = ModelState });

            var employee = await _authService.ValidateEmployeeCredentials(model.Email, model.Password);

            if (employee == null)
                return Unauthorized(new { message = "Email ou senha incorretos" });

            var token = _tokenService.GenerateEmployeeToken(employee);

            return Ok(new
            {
                id = employee.Id,
                name = employee.FullName,
                email = employee.Email,
                role = employee.Manager ? "manager" : "employee",
                isManager = employee.Manager,
                token = token
            });
        }

    }
}