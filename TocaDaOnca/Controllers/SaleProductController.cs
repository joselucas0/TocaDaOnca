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
    public class SaleProductController : ControllerBase
    {
         private readonly Context _context;

        public SaleProductController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleProduct>>> Get()
        {
            try
            {
                var saleProduct = await _context.SalesProducts.ToListAsync();

                if (saleProduct == null || !saleProduct.Any())
                {
                    return NotFound("Nenhuma venda de produto encontrada.");
                }

                return Ok(saleProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter as vendas de produto: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SaleProduct>> GetById(int id)
        {
            try
            {
                var saleProduct = await _context.SalesProducts.FindAsync(id);
                if (saleProduct == null)
                    return NotFound("Nenhuma venda de produto encontrada.");

                return Ok(saleProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter a venda de produto: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<SaleProduct>> Post([FromBody] SaleProduct saleProduct)
        {
            try
            {
                _context.SalesProducts.Add(saleProduct);
                await _context.SaveChangesAsync();
                return Ok(saleProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar a venda de produto: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SaleProduct>> Put(int id, [FromBody] SaleProduct saleProduct)
        {
            try
            {
                var existente = await _context.SalesProducts.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhuma venda de produto encontrada.");
                }
                existente.SaleId = saleProduct.SaleId;
                existente.ProductId = saleProduct.ProductId;
                existente.Quantity = saleProduct.Quantity;
                existente.CreatedAt = saleProduct.CreatedAt;
                existente.UpdatedAt = saleProduct.UpdatedAt;
                await _context.SaveChangesAsync();
                return Ok(existente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar a venda de produto: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existente = await _context.SalesProducts.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhuma venda de produto encontrada.");
                }
                _context.SalesProducts.Remove(existente);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar a venda de produto: {ex.Message}");
            }
        }

    }
}