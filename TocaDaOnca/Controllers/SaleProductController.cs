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
    public class SaleProductController : ControllerBase
    {
        private readonly Context _context;

        public SaleProductController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleProductReadDto>>> Get()
        {
            try
            {
                var saleProduct = await _context.SalesProducts.ToListAsync();

                if (saleProduct == null || !saleProduct.Any())
                {
                    return NotFound("Nenhuma venda de produto encontrada.");
                }

                var dtoList = saleProduct.Select(s => new SaleProductReadDto
                {
                    Id = s.Id,
                    SaleId = s.SaleId,
                    ProductId = s.ProductId,
                    Quantity = s.Quantity,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                });

                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter as vendas de produto: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SaleProductReadDto>> GetById(int id)
        {
            try
            {
                var saleProduct = await _context.SalesProducts.FindAsync(id);
                if (saleProduct == null)
                    return NotFound("Nenhuma venda de produto encontrada.");

                var dto = new SaleProductReadDto
                {
                    Id = saleProduct.Id,
                    SaleId = saleProduct.SaleId,
                    ProductId = saleProduct.ProductId,
                    Quantity = saleProduct.Quantity,
                    CreatedAt = saleProduct.CreatedAt,
                    UpdatedAt = saleProduct.UpdatedAt
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter a venda de produto: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<SaleProductCreateDto>> Post([FromBody] SaleProductCreateDto dto)
        {
            try
            {
                var entity = new SaleProduct
                {
                    SaleId = dto.SaleId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.SalesProducts.Add(entity);
                await _context.SaveChangesAsync();

                var result = new SaleProductReadDto
                {
                    Id = entity.Id,
                    SaleId = entity.SaleId,
                    ProductId = entity.ProductId,
                    Quantity = entity.Quantity,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt
                };
                return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar a venda de produto: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SaleProductUpdateDto>> Put(int id, [FromBody] SaleProductUpdateDto dto)
        {
            try
            {
                var existente = await _context.SalesProducts.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhuma venda de produto encontrada.");
                }

                existente.SaleId = dto.SaleId;
                existente.ProductId = dto.ProductId;
                existente.Quantity = dto.Quantity;
                existente.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var result = new SaleProductReadDto
                {
                    Id = existente.Id,
                    SaleId = existente.SaleId,
                    ProductId = existente.ProductId,
                    Quantity = existente.Quantity,
                    CreatedAt = existente.CreatedAt,
                    UpdatedAt = existente.UpdatedAt
                };

                return Ok(result);
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