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
    public class VisitorController : ControllerBase
    {
        private readonly Context _context;

        public VisitorController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitorReadDto>>> Get()
        {
            try
            {
                var visitors = await _context.Visitors.ToListAsync();

                if (visitors == null || !visitors.Any())
                {
                    return NotFound("Nenhum visitante encontrado.");
                }

                var readDto = visitors.Select(s => new VisitorReadDto
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    BirthDate = s.BirthDate,
                    Phone = s.Phone,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                });

                return Ok(readDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter os visitantes: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VisitorReadDto>> GetById(int id)
        {
            try
            {
                var visitor = await _context.Visitors.FindAsync(id);
                if (visitor == null)
                    return NotFound("Nenhum visitante encontrado.");

                var readDto = new VisitorReadDto
                {
                    Id = visitor.Id,
                    FullName = visitor.FullName,
                    BirthDate = visitor.BirthDate,
                    Phone = visitor.Phone,
                    CreatedAt = visitor.CreatedAt,
                    UpdatedAt = visitor.UpdatedAt
                };

                return Ok(readDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter o visitante: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<VisitorReadDto>> Post([FromBody] VisitorCreateDto dto)
        {
            try
            {
                var entity = new Visitor
                {
                    FullName = dto.FullName,
                    BirthDate = dto.BirthDate,
                    Phone = dto.Phone,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Visitors.Add(entity);
                await _context.SaveChangesAsync();

                var readDto = new VisitorReadDto
                {
                    Id = entity.Id,
                    FullName = entity.FullName,
                    BirthDate = entity.BirthDate,
                    Phone = entity.Phone,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt
                };
                return Ok(readDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar o visitante: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VisitorReadDto>> Put(int id, [FromBody] VisitorUpdateDto dto)
        {
            try
            {
                var existente = await _context.Visitors.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhum visitante encontrado.");
                }
                existente.FullName = dto.FullName ?? existente.FullName;
                existente.BirthDate = dto.BirthDate ?? existente.BirthDate;
                existente.Phone = dto.Phone ?? existente.Phone;
                existente.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var readDto = new VisitorReadDto
                {
                    Id = existente.Id,
                    FullName = existente.FullName,
                    BirthDate = existente.BirthDate,
                    Phone = existente.Phone,
                    CreatedAt = existente.CreatedAt,
                    UpdatedAt = existente.UpdatedAt
                };
                return Ok(readDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar o visitante: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existente = await _context.Visitors.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhum visitante encontrado.");
                }
                _context.Visitors.Remove(existente);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar o visitante: {ex.Message}");
            }
        }
    }
}