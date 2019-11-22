using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appraiser.Api.Services;
using Appraiser.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appraiser.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsibilitiesController : ControllerBase
    {
        private readonly ResponsibilityService _service;

        public ResponsibilitiesController(ResponsibilityService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponsibilityDTO>>> GetResponsibility()
        {
            return await _service.GetAll();
        }

        [HttpGet("mine")]
        public async Task<ActionResult<IEnumerable<ResponsibilityDTO>>> GetMyResponsibility()
        {
            var (found, result) = await _service.GetByUser();

            if (!found) return NotFound();

            return result.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponsibilityDTO>> GetResponsibility(int id)
        {
            var (found, dto) = await _service.Get(id);

            if (!found) return NotFound();

            return dto;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> PutResponsibility(int id, ResponsibilityDTO dto)
        {
            var found = await _service.Update(id, dto);

            if (!found) return NotFound();

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<ResponsibilityDTO>> PostResponsibility(ResponsibilityDTO dto)
        {
            dto = await _service.Save(dto);

            return CreatedAtAction("GetResponsibility", new { id = dto.Id }, dto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponsibilityDTO>> DeleteResponsibility(int id)
        {
            var (found, dto) = await _service.Delete(id);

            if (!found) return NotFound();

            return dto;
        }
    }
}