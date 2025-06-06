using Microsoft.AspNetCore.Mvc;
using TocaDaOnca.Models;
using TocaDaOnca.Models.DTO;
using TocaDaOnca.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TocaDaOnca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KioskController : ControllerBase
    {
        private readonly Context _context;

        public KioskController(Context context)
        {
            _context = context;
        }

        #region Get
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KioskReadDto>>> Get()
        {
            try
            {
                var kiosks = await _context.Kiosks.ToListAsync();

                if (kiosks == null || !kiosks.Any())
                {
                    return NotFound("Nenhum quiosque encontrado.");
                }

                var KioskDto = kiosks.Select(k => new KioskReadDto
                {
                    Id = k.Id,
                    Title = k.Title,
                    MaxPeople = k.MaxPeople,
                    Description = k.Description,
                    Value = k.Value,
                    CreatedAt = k.CreatedAt,
                    UpdatedAt = k.UpdatedAt
                });

                return Ok(KioskDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter os quiosques: {ex.Message}");
            }
        }
        #endregion
        #region GetById
        [HttpGet("{id}")]
        public async Task<ActionResult<KioskReadDto>> GetById(int id)
        {
            try
            {
                var kiosk = await _context.Kiosks.FindAsync(id);
                if (kiosk == null)
                    return NotFound("Nenhum quiosque encontrado.");

                var dto = new KioskReadDto
                {
                    Id = kiosk.Id,
                    Title = kiosk.Title,
                    MaxPeople = kiosk.MaxPeople,
                    Description = kiosk.Description,
                    Value = kiosk.Value,
                    CreatedAt = kiosk.CreatedAt,
                    UpdatedAt = kiosk.UpdatedAt
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter o quiosque: {ex.Message}");
            }
        }
        #endregion
        #region Post
        [HttpPost]
        public async Task<ActionResult<KioskReadDto>> Post([FromBody] KioskCreateDto dto)
        {
            try
            {
                var kiosk = new Kiosk
                {
                    Title = dto.Title,
                    MaxPeople = dto.MaxPeople,
                    Description = dto.Description,
                    Value = dto.Value,
                };

                _context.Kiosks.Add(kiosk);
                await _context.SaveChangesAsync();

                var readDto = new KioskReadDto
                {
                    Id = kiosk.Id,
                    Title = kiosk.Title,
                    MaxPeople = kiosk.MaxPeople,
                    Description = kiosk.Description,
                    Value = kiosk.Value,
                    CreatedAt = kiosk.CreatedAt,
                    UpdatedAt = kiosk.UpdatedAt
                };

                return Ok(dto);
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar o quiosque: {ex.Message}");
            }
        }
        #endregion
        #region Put
        [HttpPut("{id}")]
        public async Task<ActionResult<KioskReadDto>> Put(int id, [FromBody] KioskUpdateDto dto)
        {
            try
            {
                var kiosk = await _context.Kiosks.FindAsync(id);
                if (kiosk == null)
                {
                    return NotFound("Nenhum quiosque encontrado.");
                }

                if (dto.MaxPeople.HasValue)
                {
                    kiosk.MaxPeople = dto.MaxPeople.Value;
                }
                if (dto.Title != null)
                {
                    kiosk.Title = dto.Title;
                }
                if (dto.Description != null)
                {
                    kiosk.Description = dto.Description;
                }

                kiosk.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var readDto = new KioskReadDto
                {

                    Id = kiosk.Id,
                    Title = kiosk.Title,
                    MaxPeople = kiosk.MaxPeople,
                    Description = kiosk.Description,
                    Value = kiosk.Value,
                    CreatedAt = kiosk.CreatedAt,
                    UpdatedAt = kiosk.UpdatedAt
                };

                return Ok(readDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar o quiosque: {ex.Message}");
            }
        }
        #endregion
        #region Post
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var kiosk = await _context.Kiosks.FindAsync(id);
                if (kiosk == null)
                {
                    return NotFound("Nenhum quiosque encontrado.");
                }
                _context.Kiosks.Remove(kiosk);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar o quiosque: {ex.Message}");
            }
        }
        #endregion
    }
}