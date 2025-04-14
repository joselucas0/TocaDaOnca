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
    public class SaleController : ControllerBase
    {
        private readonly Context _context;

        public SaleController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleReadDto>>> Get()
        {
            try
            {
                var sale = await _context.Sales.ToListAsync();

                if (sale == null || !sale.Any())
                {
                    return NotFound("Nenhuma venda encontrada.");
                }

                var dtoList = sale.Select(s => new SaleReadDto
                {
                    Id = s.Id,
                    ReservationId = s.ReservationId,
                    EmployeeId = s.EmployeeId,
                    Subtotal = s.Subtotal,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                });

                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter as vendas: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SaleReadDto>> GetById(int id)
        {
            try
            {
                var sale = await _context.Sales.FindAsync(id);
                if (sale == null)
                    return NotFound("Nenhuma venda encontrada.");

                var dto = new SaleReadDto
                {
                    Id = sale.Id,
                    ReservationId = sale.ReservationId,
                    EmployeeId = sale.EmployeeId,
                    Subtotal = sale.Subtotal,
                    CreatedAt = sale.CreatedAt,
                    UpdatedAt = sale.UpdatedAt
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter a venda: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<SaleCreateDto>> Post([FromBody] SaleCreateDto dto)
        {
            try
            {
                var entity = new Sale
                {
                    ReservationId = dto.ReservationId,
                    EmployeeId = dto.EmployeeId,
                    Subtotal = dto.Subtotal,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Sales.Add(entity);
                await _context.SaveChangesAsync();

                var result = new SaleReadDto
                {
                    Id = entity.Id,
                    ReservationId = entity.ReservationId,
                    EmployeeId = entity.EmployeeId,
                    Subtotal = entity.Subtotal,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt
                };

                return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar a venda: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SaleUpdateDto>> Put(int id, [FromBody] SaleUpdateDto dto)
        {
            try
            {
                var existente = await _context.Sales.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhuma venda encontrada.");
                }
                if (dto.EmployeeId.HasValue)
                    existente.EmployeeId = dto.EmployeeId.Value;

                if (dto.Subtotal.HasValue)
                    existente.Subtotal = dto.Subtotal.Value;

                existente.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var result = new SaleReadDto
                {
                    Id = existente.Id,
                    ReservationId = existente.ReservationId,
                    EmployeeId = existente.EmployeeId,
                    Subtotal = existente.Subtotal,
                    CreatedAt = existente.CreatedAt,
                    UpdatedAt = existente.UpdatedAt
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar a venda: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existente = await _context.Sales.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhuma venda encontrada.");
                }
                _context.Sales.Remove(existente);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar a venda: {ex.Message}");
            }
        }
    }
}