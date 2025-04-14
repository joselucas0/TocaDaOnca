using Microsoft.AspNetCore.Mvc;
using TocaDaOnca.Models;
using TocaDaOnca.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TocaDaOnca.Models.DTO;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TocaDaOnca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly Context _context;

        public ProductController(Context context)
        {
            _context = context;
        }

        #region Get
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> Get()
        {
            try
            {
                var products = await _context.Products.ToListAsync();

                if (products == null || !products.Any())
                {
                    return NotFound("Nenhum produto encontrado.");
                }

                var productsDto = products.Select(p => new ProductReadDto
                {
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Cost = p.Cost,
                    Price = p.Price,
                    Stock = p.Stock,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                });

                return Ok(productsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter os produtos: {ex.Message}");
            }
        }
        #endregion

        #region GetById
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return NotFound("Nenhum produto encontrado.");

                var productsDto = new ProductReadDto
                {
                    ProductName = product.ProductName,
                    Description = product.Description,
                    Cost = product.Cost,
                    Price = product.Price,
                    Stock = product.Stock,
                    CreatedAt = product.CreatedAt,
                    UpdatedAt = product.UpdatedAt
                };

                return Ok(productsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter o produto: {ex.Message}");
            }
        }
        #endregion

        #region Post
        [HttpPost]
        public async Task<ActionResult<Product>> Post([FromBody] ProductCreateDto dto)
        {
            try
            {
                var product = new Product
                {
                    ProductName = dto.ProductName,
                    Description = dto.Description,
                    Cost = dto.Cost,
                    Price = dto.Price,
                    Stock = dto.Stock,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                var readDto = new ProductReadDto
                {
                    ProductName = dto.ProductName,
                    Description = dto.Description,
                    Cost = dto.Cost,
                    Price = dto.Price,
                    Stock = dto.Stock
                };
                return Ok(readDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar o produto: {ex.Message}");
            }
        }
        #endregion

        #region Put
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Put(int id, [FromBody] ProductUpdateDto dto)
        {
            try
            {
                var existente = await _context.Products.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhum produto encontrado.");
                }

                

                if (dto.ProductName != null)
                {
                    existente.ProductName = dto.ProductName;
                }
                if (dto.Description != null)
                {
                    existente.Description = dto.Description;
                }
                if (dto.Cost.HasValue)
                {
                    existente.Cost = dto.Cost.Value;
                }
                if (dto.Price.HasValue)
                {
                    existente.Price = dto.Price.Value;
                }
                if (dto.Stock.HasValue)
                {
                    existente.Stock = dto.Stock.Value;
                }

                existente.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Ok(existente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar o produto: {ex.Message}");
            }
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existente = await _context.Products.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhum produto encontrado.");
                }
                _context.Products.Remove(existente);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar o produto: {ex.Message}");
            }
        }
        #endregion
    }
}