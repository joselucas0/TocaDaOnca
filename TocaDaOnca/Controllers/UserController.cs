using Microsoft.AspNetCore.Mvc;
using TocaDaOnca.Models;
using TocaDaOnca.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TocaDaOnca.Models.DTO;

namespace TocaDaOnca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly Context _context;

        public UserController(Context context)
        {
            _context = context;
        }

        // GET: api/User/profile
        [HttpGet("profile")]
        [Authorize] // Esta rota requer autenticação
        public async Task<ActionResult<UserReadDto>> GetUserProfile()
        {
            // Obtém o ID do usuário autenticado
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
            {
                return BadRequest("ID de usuário inválido ou não encontrado no token");
            }

            // Busca o usuário pelo ID
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var readDto = new UserReadDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Cpf = user.Cpf,
                BirthDate = user.BirthDate,
                Email = user.Email,
                Phone = user.Phone,
                Premium = user.Premium,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return Ok(readDto);
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetUsers()
        {
            try
            {
                var user = await _context.Users.ToListAsync();
                if (user == null || !user.Any())
                {
                    return NotFound("nenhum usuario encontrado.");
                }

                var readDto = user.Select(u => new UserReadDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Cpf = u.Cpf,
                    BirthDate = u.BirthDate,
                    Email = u.Email,
                    Phone = u.Phone,
                    Premium = u.Premium,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt
                });

                return Ok(readDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro os usuarios: {ex.Message}");
            }
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDto>> GetById(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound("Nenhum usuario encontrado");
                }

                var readDto = new UserReadDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Cpf = user.Cpf,
                    BirthDate = user.BirthDate,
                    Email = user.Email,
                    Phone = user.Phone,
                    Premium = user.Premium,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                };

                return Ok(readDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter o usuario: {ex.Message}");
            }


        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<UserReadDto>> Post([FromBody] UserCreateDto dto)
        {
            try
            {
                var entity = new User
                {
                    FullName = dto.FullName,
                    Cpf = dto.Cpf,
                    BirthDate = dto.BirthDate,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Premium = dto.Premium,
                    Password = dto.Password
                };
                _context.Users.Add(entity);
                await _context.SaveChangesAsync();

                var readDto = new UserReadDto
                {
                    Id = entity.Id,
                    FullName = entity.FullName,
                    Cpf = entity.Cpf,
                    BirthDate = entity.BirthDate,
                    Email = entity.Email,
                    Phone = entity.Phone,
                    Premium = entity.Premium,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt
                };
                return Ok(readDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar usuario: {ex.Message}");
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<ActionResult<UserReadDto>> Put(int id, UserUpdateDto dto)
        {
            try
            {
                var existente = await _context.Users.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhum usuario encontrado.");
                }

                existente.FullName = dto.FullName ?? existente.FullName;
                existente.Phone = dto.Phone ?? existente.Phone;
                existente.Premium = dto.Premium ?? existente.Premium;
                existente.BirthDate = dto.BirthDate ?? existente.BirthDate;
                existente.UpdatedAt = DateTime.UtcNow;

                var readDto = new UserReadDto
                {
                    Id = existente.Id,
                    FullName = existente.FullName,
                    Cpf = existente.Cpf,
                    BirthDate = existente.BirthDate,
                    Email = existente.Email,
                    Phone = existente.Phone,
                    Premium = existente.Premium,
                    CreatedAt = existente.CreatedAt,
                    UpdatedAt = existente.UpdatedAt
                };

                return Ok(readDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar usuario: {ex.Message}");
            }

        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}