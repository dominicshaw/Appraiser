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

namespace Appraiser.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingController : ControllerBase
    {
        private readonly AppraiserContext _context;
        private readonly TrainingMapper _mapper;

        public TrainingController(AppraiserContext context, TrainingMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingDTO>>> GetTraining()
        {
            return (await _context.Training.ToListAsync()).Select(x => _mapper.ToDTO(x, new TrainingDTO())).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingDTO>> GetTraining(int id)
        {
            var training = await _context.Training.FindAsync(id);

            if (training == null)
                return NotFound();

            return _mapper.ToDTO(training, new TrainingDTO());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTraining(int id, TrainingDTO dto)
        {
            var training = _mapper.ToModel(dto, new Training());

            if (id != training.Id)
                return BadRequest();

            _context.Entry(training).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingExists(id))
                    return NotFound();
                
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<TrainingDTO>> PostTraining(TrainingDTO dto)
        {
            var training = _mapper.ToModel(dto, new Training());

            _context.Training.Add(training);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTraining", new { id = training.Id }, _mapper.ToDTO(training, new TrainingDTO()));
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<TrainingDTO>> DeleteTraining(int id)
        {
            var training = await _context.Training.FindAsync(id);
            
            if (training == null)
                return NotFound();

            _context.Training.Remove(training);
            await _context.SaveChangesAsync();

            return _mapper.ToDTO(training, new TrainingDTO());
        }

        private bool TrainingExists(int id)
        {
            return _context.Training.Any(e => e.Id == id);
        }
    }
}
