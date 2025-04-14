using Microsoft.AspNetCore.Mvc;
using TocaDaOnca.Models;
using TocaDaOnca.Models.DTO;
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

        #region Get
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
        #endregion

        #region GetById
        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationReadDto>> GetById(int id)
        {
            try
            {
                var reservation = await _context.Reservations.FindAsync(id);
                if (reservation == null)
                    return NotFound("Nenhuma reserva encontrada.");

                var dto = new ReservationReadDto
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
        #endregion

        #region Post
        [HttpPost]
        public async Task<ActionResult<ReservationReadDto>> Post([FromBody] ReservationCreateDto dto)
        {
            try
            {

                var reservation = new Reservation
                {
                    UserId = dto.UserId,
                    KioskId = dto.KioskId,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                var readDto = new ReservationReadDto
                {
                    Id = reservation.Id,
                    UserId = reservation.UserId,
                    KioskId = reservation.KioskId,
                    StartTime = reservation.StartTime,
                    EndTime = reservation.EndTime,
                    CreatedAt = reservation.CreatedAt,
                    UpdatedAt = reservation.UpdatedAt
                };

                return readDto;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar a reserva: {ex.Message}");
            }
        }
        #endregion

        #region Put
        [HttpPut("{id}")]
        public async Task<ActionResult<ReservationReadDto>> Put(int id, [FromBody] ReservationUpdateDto dto)
        {
            try
            {
                var existente = await _context.Reservations.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhuma reserva encontrada.");
                }

                // Checa o Kiosk
                if (dto.KioskId.HasValue)
                {
                    existente.KioskId = dto.KioskId.Value;
                }

                // Checa os horários    
                if (dto.StartTime.HasValue)
                {
                    existente.StartTime = dto.StartTime.Value;
                }

                if (dto.EndTime.HasValue)
                {
                    existente.EndTime = dto.EndTime.Value;
                }

                existente.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var readDto = new ReservationReadDto
                {
                    UserId = existente.UserId,
                    KioskId = existente.KioskId,
                    StartTime = existente.StartTime,
                    EndTime = existente.EndTime,
                    CreatedAt = existente.CreatedAt,
                    UpdatedAt = existente.UpdatedAt
                };

                return Ok(readDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar a reserva: {ex.Message}");
            }
        }
        #endregion

        #region Delete
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
        #endregion

    }
}