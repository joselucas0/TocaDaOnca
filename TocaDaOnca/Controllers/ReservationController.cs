using Microsoft.AspNetCore.Mvc;
using TocaDaOnca.Models;
using TocaDaOnca.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Resources;


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
        public async Task<ActionResult<IEnumerable<ReservationReadDto>>> Get()
        {
            try
            {
                var reservation = await _context.Reservations.ToListAsync();

                if (reservation == null || !reservation.Any())
                {
                    return NotFound("Nenhuma reserva encontrada.");
                }

                // Aqui está sendo criado um novo objeto para poder retornar com os campos corretos
                var reservationDtos = reservation.Select(r => new ReservationReadDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    KioskId = r.KioskId,
                    StartTime = r.StartTime,
                    EndTime = r.EndTime,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                });

                return Ok(reservationDtos);
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

                var dto = new reservationReadDto
                {
                    Id = reservation.Id,
                    UserId = reservation.UserId,
                    KioskId = reservation.KioskId,
                    StartTime = reservation.StartTime,
                    EndTime = reservation.EndTime,
                    CreatedAt = reservation.CreatedAt,
                    UpdatedAt = reservation.UpdatedAt
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter a reserva: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Reservation>> Post([FromBody] ReservationCreateDto dto)
        {
            try
            {
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                var createdDto = new Reservation
                {
                    UserId = dto.UserId,
                    KioskId = dto.KioskId,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.AddAsync(createdDto);
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