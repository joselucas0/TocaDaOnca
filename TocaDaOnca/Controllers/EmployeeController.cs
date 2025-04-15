using Microsoft.AspNetCore.Mvc;
using TocaDaOnca.Models;
using TocaDaOnca.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TocaDaOnca.Services;
using TocaDaOnca.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;

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

        #region GetEmployeeProfile
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
        #endregion

        #region GetDashboard
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
        #endregion

        #region Get
        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeReadDto>>> GetEmployees()
        {
            try
            {
                var employees = await _context.Employees.ToListAsync();

                // Não retornar as senhas no resultado
                foreach (var employee in employees)
                {
                    employee.Password = string.Empty;
                }

                var readDto = employees.Select(s => new EmployeeReadDto
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    Cpf = s.Cpf,
                    Email = s.Email,
                    Manager = s.Manager,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                });


                return Ok(readDto);
            }
            catch (Exception ex)
            {
                {
                    return StatusCode(500, $"Erro ao obter os Empregados: {ex.Message}");
                }
            }
        }
        #endregion

        #region GetById
        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeReadDto>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound("Nenhum empregado encontrado.");
            }

            var readDto = new EmployeeReadDto
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Cpf = employee.Cpf,
                Email = employee.Email,
                Manager = employee.Manager,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt
            };

            return Ok(readDto);
        }
        #endregion

        #region POST
        // POST: api/Employee
        [HttpPost]
        public async Task<ActionResult<EmployeeReadDto>> PostEmployee(EmployeeCreateDto dto)
        {
            try
            {
                var employee = new Employee
                {
                    FullName = dto.FullName,
                    Cpf = dto.Cpf,
                    Email = dto.Email,
                    Manager = dto.Manager
                };
                string Password = BCrypt.Net.BCrypt.HashPassword(employee.Password);

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                var readDto = new EmployeeReadDto
                {
                    Id = employee.Id,
                    FullName = employee.FullName,
                    Cpf = employee.Cpf,
                    Email = employee.Email,
                    Manager = employee.Manager,
                    CreatedAt = employee.CreatedAt,
                    UpdatedAt = employee.UpdatedAt
                };

                return readDto;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao criar empregado: " + ex.Message);
            }
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeReadDto>> PutEmployee(int id, [FromBody] EmployeeUpdateDto employee)
        {
            try
            {
                var existingEmployee = await _context.Employees.FindAsync(id);
                if (existingEmployee == null)
                    return NotFound("Not found employeer");
                

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

                if (employee.FullName != null)
                    existingEmployee.FullName = employee.FullName;
                
                if (employee.Cpf != null)
                    existingEmployee.Cpf = employee.Cpf;

                if (employee.Email != null)
                    existingEmployee.Email = employee.Email;

                existingEmployee.UpdatedAt = DateTime.UtcNow;

                // _context.Entry(employee).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                var readDto = new EmployeeReadDto
                {
                    FullName = existingEmployee.FullName,
                    Cpf = existingEmployee.Cpf,
                    Email = existingEmployee.Email,
                    Manager = existingEmployee.Manager,
                    CreatedAt = existingEmployee.CreatedAt,
                    UpdatedAt = existingEmployee.UpdatedAt
                };

                return Ok(readDto);
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
            } catch (Exception ex)
            {
                return StatusCode(500, "Erro ao atualizar empregado: " + ex.Message);
            }

        }
        #endregion

        #region Delete
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

        #endregion

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

        #region TestEmployeeAccess
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

        //GET: api/Employee/test/manager-access
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
        #endregion
    }
}