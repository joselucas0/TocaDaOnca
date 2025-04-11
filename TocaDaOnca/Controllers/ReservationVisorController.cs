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
    public class ReservationVisorController : ControllerBase
    {
        private readonly Context _context;

        public ReservationVisorController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationVisitor>>> Get()
        {
            try
            {
                var reservationVisitor = await _context.ReservationVisitors.ToListAsync();

                if (reservationVisitor == null || !reservationVisitor.Any())
                {
                    return NotFound("Nenhuma reserva de visitante encontrada.");
                }

                return Ok(reservationVisitor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter a reservas de visitante: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationVisitor>> GetById(int id)
        {
            try
            {
                var reservationVisitor = await _context.ReservationVisitors.FindAsync(id);
                if (reservationVisitor == null)
                    return NotFound("Nenhuma reserva de visitante encontrada.");

                return Ok(reservationVisitor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter a reserva de visitante: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ReservationController>> Post([FromBody] ReservationVisitor reservationVisitor)
        {
            try
            {
                _context.ReservationVisitors.Add(reservationVisitor);
                await _context.SaveChangesAsync();
                return Ok(reservationVisitor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar a reserva de visitante: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReservationController>> Put(int id, [FromBody] ReservationVisitor reservationVisitor)
        {
            try
            {
                var existente = await _context.ReservationVisitors.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhuma reserva de visitante encontrada.");
                }
                existente.ReservationId = reservationVisitor.ReservationId;
                existente.VisitorId = reservationVisitor.VisitorId;
                existente.CreatedAt = reservationVisitor.CreatedAt;
                existente.UpdatedAt = reservationVisitor.UpdatedAt;
                await _context.SaveChangesAsync();
                return Ok(existente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar a reserva de visitante encontrada: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existente = await _context.ReservationVisitors.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhuma reserva de visitante encontrada.");
                }
                _context.ReservationVisitors.Remove(existente);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar a reserva de visitante: {ex.Message}");
            }
        }
    }
}