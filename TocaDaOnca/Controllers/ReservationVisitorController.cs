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
    public class ReservationVisitorController : ControllerBase
    {
        private readonly Context _context;

        public ReservationVisitorController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationVisitorReadDto>>> Get()
        {
            try
            {
                var reservationVisitors = await _context.ReservationVisitors.ToListAsync();

                if (reservationVisitors == null || !reservationVisitors.Any())
                    return NotFound("Nenhuma reserva de visitante encontrada.");

                var readDto = reservationVisitors.Select(rv => new ReservationVisitorReadDto
                {
                    Id = rv.Id,
                    ReservationId = rv.ReservationId,
                    VisitorId = rv.VisitorId,
                    CreatedAt = rv.CreatedAt,
                    UpdatedAt = rv.UpdatedAt,
                });

                return Ok(readDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter as reservas de visitante: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationVisitorReadDto>> GetById(int id)
        {
            try
            {
                var rv = await _context.ReservationVisitors.FindAsync(id);
                if (rv == null)
                    return NotFound("Nenhuma reserva de visitante encontrada.");

                var readDto = new ReservationVisitorReadDto
                {
                    Id = rv.Id,
                    ReservationId = rv.ReservationId,
                    VisitorId = rv.VisitorId,
                    CreatedAt = rv.CreatedAt,
                    UpdatedAt = rv.UpdatedAt
                };

                return Ok(readDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter a reserva de visitante: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ReservationVisitorReadDto>> Post([FromBody] ReservationVisitorCreateDto dto)
        {
            try
            {
                var entity = new ReservationVisitor
                {
                    ReservationId = dto.ReservationId,
                    VisitorId = dto.VisitorId
                };

                _context.ReservationVisitors.Add(entity);
                await _context.SaveChangesAsync();

                var readDto = new ReservationVisitorReadDto
                {
                    Id = entity.Id,
                    ReservationId = entity.ReservationId,
                    VisitorId = entity.VisitorId,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt
                };

                return Ok(readDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar a reserva de visitante: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReservationVisitorReadDto>> Put(int id, [FromBody] ReservationVisitorUpdateDto dto)
        {
            try
            {
                var existente = await _context.ReservationVisitors.FindAsync(id);
                if (existente == null)
                    return NotFound("Nenhuma reserva de visitante encontrada.");

                existente.ReservationId = dto.ReservationId ?? existente.ReservationId;
                existente.VisitorId = dto.VisitorId ?? existente.VisitorId;

                existente.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var readDto = new ReservationVisitorReadDto
                {
                    Id = existente.Id,
                    ReservationId = existente.ReservationId,
                    VisitorId = existente.VisitorId,
                    CreatedAt = existente.CreatedAt,
                    UpdatedAt = existente.UpdatedAt
                };

                return Ok(readDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar a reserva de visitante: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existente = await _context.ReservationVisitors.FindAsync(id);
                if (existente == null)
                    return NotFound("Nenhuma reserva de visitante encontrada.");

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