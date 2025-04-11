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
    public class ProductController : ControllerBase
    {
        private readonly Context _context;

        public ProductController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            try
            {
                var products = await _context.Products.ToListAsync();

                if (products == null || !products.Any())
                {
                    return NotFound("Nenhum produto encontrado.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter os produtos: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return NotFound("Nenhum produto encontrado.");

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter o produto: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post([FromBody] Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar o produto: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Put(int id, [FromBody] Product product)
        {
            try
            {
                var existente = await _context.Products.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhum produto encontrado.");
                }
                existente.Name = product.Name;
                existente.Description = product.Description;
                existente.Cost = product.Cost;
                existente.Price = product.Price;
                existente.Stock = product.Stock;
                existente.CreatedAt = product.CreatedAt;
                existente.UpdatedAt = product.UpdatedAt;
                await _context.SaveChangesAsync();
                return Ok(existente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar o produto: {ex.Message}");
            }
        }

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
    }
}