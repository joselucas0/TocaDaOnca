using Microsoft.AspNetCore.Mvc;
using TocaDaOnca.Models;
using TocaDaOnca.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        public async Task<ActionResult<IEnumerable<Visitor>>> Get()
        {
            try
            {
                var visitors = await _context.Visitors.ToListAsync();

                if (visitors == null || !visitors.Any())
                {
                    return NotFound("Nenhum visitante encontrado.");
                }

                return Ok(visitors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter os visitantes: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Visitor>> GetById(int id)
        {
            try
            {
                var visitor = await _context.Visitors.FindAsync(id);
                if (visitor == null)
                    return NotFound("Nenhum visitante encontrado.");

                return Ok(visitor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter o visitante: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Visitor>> Post([FromBody] Visitor visitors)
        {
            try
            {
                _context.Visitors.Add(visitors);
                await _context.SaveChangesAsync();
                return Ok(visitors);
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
                existente.Name = visitor.Name;
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