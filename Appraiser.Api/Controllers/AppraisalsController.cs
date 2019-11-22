using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appraiser.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Appraiser.DTOs;
using Appraiser.Reporting;
using Microsoft.AspNetCore.Authorization;

namespace Appraiser.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AppraisalsController : ControllerBase
    {
        private readonly AppraisalService _service;
        private readonly ReportManager _reportManager;

        public AppraisalsController(AppraisalService service, ReportManager reportManager)
        {
            _service = service;
            _reportManager = reportManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppraisalDTO>>> GetAppraisals()
        {
            return (await _service.GetAll()).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppraisalDTO>> GetAppraisal(int id)
        {
            var (found, dto) = await _service.Get(id);

            if (!found) return NotFound();

            return dto;
        }

        [HttpGet("/printable/{id}")]
        public async Task<FileStreamResult> GetAppraisalPdf(int id)
        {
            var (_, dto) = await _service.Get(id);

            return File(_reportManager.GetAppraisal(dto), "application/pdf", $"{dto.Staff.Name}, {dto.PeriodStart.Year}.pdf");
        }

        [HttpGet("mine")]
        public async Task<ActionResult<IEnumerable<AppraisalDTO>>> GetMyAppraisals()
        {
            var (found, result) = await _service.GetByUser();

            if (!found) return NotFound();

            return result.ToList();
        }

        [HttpGet("managed")]
        public async Task<ActionResult<IEnumerable<AppraisalDTO>>> GetManagedAppraisals()
        {
            return (await _service.GetByManager()).ToList();
        }

        [HttpGet("clone/{id}/{year}")]
        public async Task<ActionResult<AppraisalDTO>> CloneAppraisal(int id, int year)
        {
            return await _service.Clone(id, year);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> PutAppraisal(int id, AppraisalDTO dto)
        {
            var found = await _service.Update(id, dto);

            if (!found) return NotFound();

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<AppraisalDTO>> PostAppraisal(AppraisalDTO dto)
        {
            dto = await _service.Save(dto);

            return CreatedAtAction("GetAppraisal", new { id = dto.Id }, dto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AppraisalDTO>> DeleteAppraisal(int id)
        {
            var (found, dto) = await _service.Delete(id);

            if (!found) return NotFound();

            return dto;
        }
    }
}
