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
    public class ReportController : ControllerBase
    {
        private readonly Context _context;

        public ReportController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Report>>> Get()
        {
            try
            {
                var report = await _context.Reports.ToListAsync();

                if (report == null || !report.Any())
                {
                    return NotFound("Nenhum relatório encontrado.");
                }

                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter os relatórios: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> GetById(int id)
        {
            try
            {
                var report = await _context.Reports.FindAsync(id);
                if (report == null)
                    return NotFound("Nenhum relatório encontrado.");

                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter o relatório: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Report>> Post([FromBody] Report report)
        {
            try
            {
                _context.Reports.Add(report);
                await _context.SaveChangesAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar o relatório: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Report>> Put(int id, [FromBody] Report report)
        {
            try
            {
                var existente = await _context.Reports.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhum relatório encontrado.");
                }
                existente.Description = report.Description;
                existente.TotalSales = report.TotalSales;
                existente.TotalRentals = report.TotalRentals;
                existente.TotalVisitors = report.TotalVisitors;
                existente.TotalCosts = report.TotalCosts;
                existente.TotalRevenue = report.TotalRevenue;
                existente.ReportType = report.ReportType;
                existente.CreatedAt = report.CreatedAt;
                existente.UpdatedAt = report.UpdatedAt;
                await _context.SaveChangesAsync();
                return Ok(existente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar o relatório: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existente = await _context.Reports.FindAsync(id);
                if (existente == null)
                {
                    return NotFound("Nenhum Relatório encontrado.");
                }
                _context.Reports.Remove(existente);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar o relatório: {ex.Message}");
            }
        }
    }
}