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
    public class ReservationController : ControllerBase
    {
        private readonly Context _context;

        public ReservationController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> Get()
        {
            try
            {
                var reservation = await _context.Reservations.ToListAsync();

                if (reservation == null || !reservation.Any())
                {
                    return NotFound("Nenhuma reserva encontrada.");
                }

                return Ok(reservation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter as reservas: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetById(int id)
        {
            try
            {
                var reservation = await _context.Reservations.FindAsync(id);
                if (reservation == null)
                    return NotFound("Nenhuma reserva encontrada.");

                return Ok(reservation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter a reserva: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Reservation>> Post([FromBody] Reservation reservation)
        {
            try
            {
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
                return Ok(reservation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar a reserva: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Reservation>> Put(int id, [FromBody] Reservation reservation)
        {
            try
            {
                var existente = await _context.Reservations.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhuma reserva encontrada.");
                }
                existente.UserId = reservation.UserId;
                existente.KioskId = reservation.KioskId;
                existente.StartTime = reservation.StartTime;
                existente.EndTime = reservation.EndTime;
                existente.CreatedAt = reservation.CreatedAt;
                existente.UpdatedAt = reservation.UpdatedAt;
                await _context.SaveChangesAsync();
                return Ok(existente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar a reserva: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existente = await _context.Reservations.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhuma reserva encontrada.");
                }
                _context.Reservations.Remove(existente);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar a reserva: {ex.Message}");
            }
        }


    }
}