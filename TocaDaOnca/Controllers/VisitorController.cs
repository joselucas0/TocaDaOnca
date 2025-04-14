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

                var dtoList = visitors.Select(s => new VisitorReadDto
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    BirthDate = s.BirthDate,
                    Phone = s.Phone,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                });

                return Ok(dtoList);
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

                var dto = new VisitorReadDto
                {
                    Id = visitor.Id,
                    FullName = visitor.FullName,
                    BirthDate = visitor.BirthDate,
                    Phone = visitor.Phone,
                    CreatedAt = visitor.CreatedAt,
                    UpdatedAt = visitor.UpdatedAt
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter o visitante: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<VisitorCreateDto>> Post([FromBody] VisitorCreateDto dto)
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

                var result = new VisitorReadDto
                {
                    Id = entity.Id,
                    FullName = entity.FullName,
                    BirthDate = entity.BirthDate,
                    Phone = entity.Phone,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt
                };
                return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar o visitante: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Visitor>> Put(int id, [FromBody] Visitor visitor)
        {
            try
            {
                var existente = await _context.Visitors.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhum visitante encontrado.");
                }
                existente.FullName = visitor.FullName;
                existente.BirthDate = visitor.BirthDate;
                existente.Phone = visitor.Phone;
                existente.CreatedAt = visitor.CreatedAt;
                existente.UpdatedAt = visitor.UpdatedAt;
                await _context.SaveChangesAsync();
                return Ok(existente);
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