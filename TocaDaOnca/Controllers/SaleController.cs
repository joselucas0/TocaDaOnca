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
    public class SaleController : ControllerBase
    {
        private readonly Context _context;

        public SaleController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> Get()
        {
            try
            {
                var sale = await _context.Sales.ToListAsync();

                if (sale == null || !sale.Any())
                {
                    return NotFound("Nenhuma venda encontrada.");
                }

                return Ok(sale);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter as vendas: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GetById(int id)
        {
            try
            {
                var sale = await _context.Sales.FindAsync(id);
                if (sale == null)
                    return NotFound("Nenhuma venda encontrada.");

                return Ok(sale);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter a venda: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Sale>> Post([FromBody] Sale sale)
        {
            try
            {
                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();
                return Ok(sale);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar a venda: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Sale>> Put(int id, [FromBody] Sale sale)
        {
            try
            {
                var existente = await _context.Sales.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhuma venda encontrada.");
                }
                existente.ReservationId = sale.ReservationId;
                existente.EmployeeId = sale.EmployeeId;
                existente.Subtotal = sale.Subtotal;
                existente.CreatedAt = sale.CreatedAt;
                existente.UpdatedAt = sale.UpdatedAt;
                existente.Reservation = sale.Reservation;
                existente.Employee = sale.Employee;
                existente.SaleProducts = sale.SaleProducts;
                await _context.SaveChangesAsync();
                return Ok(existente);
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