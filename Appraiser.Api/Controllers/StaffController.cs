using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appraiser.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Appraiser.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Appraiser.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly StaffService _service;

        public StaffController(StaffService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffDTO>>> GetStaff()
        {
            return await _service.GetAll();
        }

        [HttpGet("mine")]
        public async Task<ActionResult<IEnumerable<StaffDTO>>> GetMyStaff()
        {
            var (found, result) = await _service.GetByUser();

            if (!found) return NotFound();

            return result.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StaffDTO>> GetStaff(int id)
        {
            var (found, dto) = await _service.Get(id);

            if (!found) return NotFound();

            return dto;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> PutStaff(int id, StaffDTO dto)
        {
            var found = await _service.Update(id, dto);

            if (!found) return NotFound();

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<StaffDTO>> PostStaff(StaffDTO dto)
        {
            dto = await _service.Save(dto);

            return CreatedAtAction("GetStaff", new {id = dto.Id}, dto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<StaffDTO>> DeleteStaff(int id)
        {
            var (found, dto) = await _service.Delete(id);

            if (!found) return NotFound();

            return dto;
        }
    }
}
