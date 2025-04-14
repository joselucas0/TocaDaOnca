using Microsoft.AspNetCore.Mvc;
using TocaDaOnca.Models;
using TocaDaOnca.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TocaDaOnca.Services;

namespace TocaDaOnca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly Context _context;
        private readonly TokenService _tokenService;

        public EmployeeController(Context context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // GET: api/Employee/profile
        [HttpGet("profile")]
        [Authorize] // Esta rota requer autenticação
        public async Task<ActionResult<Employee>> GetEmployeeProfile()
        {
            // Obtém o ID do funcionário autenticado
            var employeeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(employeeId) || !int.TryParse(employeeId, out int id))
            {
                return BadRequest("ID de funcionário inválido ou não encontrado no token");
            }

            // Busca o funcionário pelo ID
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            // Não retornar a senha no resultado
            employee.Password = string.Empty;

            return employee;
        }

        // GET: api/Employee/dashboard
        [HttpGet("dashboard")]
        [Authorize(Policy = "IsManager")] // Only managers can access
        public async Task<IActionResult> GetDashboard()
        {
            // Obtém o ID do funcionário autenticado
            var employeeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(employeeId) || !int.TryParse(employeeId, out int id))
            {
                return BadRequest("ID de funcionário inválido ou não encontrado no token");
            }

            // Busca o funcionário pelo ID
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound("Funcionário não encontrado");
            }

            // Aqui você pode retornar os dados do dashboard
            return Ok(new
            {
                message = "Dashboard de gerente",
                // Adicione aqui os dados que deseja mostrar no dashboard
            });
        }


        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _context.Employees.ToListAsync();

            // Não retornar as senhas no resultado
            foreach (var employee in employees)
            {
                employee.Password = string.Empty;
            }

            return employees;
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            // Não retornar a senha no resultado
            employee.Password = string.Empty;

            return employee;
        }

        // POST: api/Employee
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            // Garante que os DateTimes estejam em UTC
            employee.CreatedAt = DateTime.UtcNow;
            employee.UpdatedAt = DateTime.UtcNow;

            // Hash da senha
            employee.Password = BCrypt.Net.BCrypt.HashPassword(employee.Password);

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Não retornar a senha no resultado
            var returnEmployee = employee;
            returnEmployee.Password = string.Empty;

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, returnEmployee);
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            // Primeiro, buscar o funcionário existente
            var existingEmployee = await _context.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (existingEmployee == null)
            {
                return NotFound();
            }

            // Se a senha estiver vazia, manter a senha atual
            if (string.IsNullOrWhiteSpace(employee.Password))
            {
                employee.Password = existingEmployee.Password;
            }
            // Se não, fazer o hash da nova senha
            else if (employee.Password != existingEmployee.Password)
            {
                employee.Password = BCrypt.Net.BCrypt.HashPassword(employee.Password);
            }

            employee.UpdatedAt = DateTime.UtcNow;

            // Se não foram enviadas datas, manter as existentes
            if (employee.CreatedAt == default)
            {
                employee.CreatedAt = existingEmployee.CreatedAt;
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

        // Add these endpoints to test role-based access

        // GET: api/Employee/test/employee-access
        [HttpGet("test/employee-access")]
        [Authorize(Policy = "IsEmployee")] // Both employees and managers can access
        public IActionResult TestEmployeeAccess()
        {
            return Ok(new
            {
                message = "Você tem acesso de funcionário",
                role = User.FindFirst(ClaimTypes.Role)?.Value,
                isManager = User.IsInRole("manager")
            });
        }

        // GET: api/Employee/test/manager-access
        [HttpGet("test/manager-access")]
        [Authorize(Policy = "IsManager")] // Only managers can access
        public IActionResult TestManagerAccess()
        {
            return Ok(new
            {
                message = "Você tem acesso de gerente",
                employeeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                name = User.FindFirst(ClaimTypes.Name)?.Value
            });
        }
    }
}