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
    public class KioskController : ControllerBase
    {
        private readonly Context _context;

        public KioskController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kiosk>>> Get()
        {
            try
            {
                var kiosks = await _context.Kiosks.ToListAsync();

                if (kiosks == null || !kiosks.Any())
                {
                    return NotFound("Nenhum quiosque encontrado.");
                }

                return Ok(kiosks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter os quiosques: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Kiosk>> GetById(int id)
        {
            try
            {
                var kiosk = await _context.Kiosks.FindAsync(id);
                if (kiosk == null)
                    return NotFound("Nenhum quiosque encontrado.");

                return Ok(kiosk);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter o quiosque: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Kiosk>> Post([FromBody] Kiosk kiosk)
        {
            try
            {
                _context.Kiosks.Add(kiosk);
                await _context.SaveChangesAsync();
                return Ok(kiosk);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar o quiosque: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Kiosk>> Put(int id, [FromBody] Kiosk kiosk)
        {
            try
            {
                var existente = await _context.Kiosks.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhum quiosque encontrado.");
                }
                existente.Title = kiosk.Title;
                existente.Max_people = kiosk.Max_people;
                existente.Description = kiosk.Description;
                existente.CreatedAt = kiosk.CreatedAt;
                existente.UpdatedAt = kiosk.UpdatedAt;
                await _context.SaveChangesAsync();
                return Ok(existente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar o quiosque: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existente = await _context.Kiosks.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhum quiosque encontrado.");
                }
                _context.Kiosks.Remove(existente);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar o quiosque: {ex.Message}");
            }
        }

    }
}