using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Appraiser.Data;
using Appraiser.Data.Models;
using Appraiser.DTOs;
using Appraiser.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Appraiser.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ObjectivesController : ControllerBase
    {
        private readonly ILogger<ObjectivesController> _log;
        private readonly AppraiserContext _context;
        private readonly ObjectiveMapper _mapper;

        public ObjectivesController(ILogger<ObjectivesController> log, AppraiserContext context, ObjectiveMapper mapper)
        {
            _log = log;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObjectiveDTO>>> GetObjectives()
        {
            return (await _context.Objectives.ToListAsync()).Select(x => _mapper.ToDTO(x, new ObjectiveDTO())).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ObjectiveDTO>> GetObjective(int id)
        {
            var objective = await _context.Objectives.FindAsync(id);

            if (objective == null)
                return NotFound();

            return _mapper.ToDTO(objective, new ObjectiveDTO());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutObjective(int id, ObjectiveDTO dto)
        {
            var objective = _mapper.ToModel(dto, new Objective());

            if (id != objective.Id)
                return BadRequest();

            _context.Entry(objective).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObjectiveExists(id))
                    return NotFound();
                
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<ObjectiveDTO>> PostObjective(ObjectiveDTO dto)
        {
            var objective = _mapper.ToModel(dto, new Objective());

            _context.Objectives.Add(objective);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetObjective", new {id = objective.Id}, _mapper.ToDTO(objective, new ObjectiveDTO()));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ObjectiveDTO>> DeleteObjective(int id)
        {
            _log.LogInformation("Delete objective {id} called", id);

            var objective = await _context.Objectives.FindAsync(id);

            if (objective == null)
            {
                _log.LogInformation("Did not find objective id {id}", id);
                return NotFound();
            }

            _context.Objectives.Remove(objective);
            var result = await _context.SaveChangesAsync();

            _log.LogInformation("Deleted objective id {id} with result {result}", id, result);

            return _mapper.ToDTO(objective, new ObjectiveDTO());
        }

        private bool ObjectiveExists(int id)
        {
            return _context.Objectives.Any(e => e.Id == id);
        }
    }
}
