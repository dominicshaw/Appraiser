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
    public class ReviewsController : ControllerBase
    {
        private readonly AppraiserContext _context;
        private readonly ReviewMapper _mapper;

        public ReviewsController(AppraiserContext context, ReviewMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetReviews()
        {
            return (await _context.Reviews.ToListAsync()).Select(x => _mapper.ToDTO(x, new ReviewDTO())).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDTO>> GetReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
                return NotFound();

            return _mapper.ToDTO(review, new ReviewDTO());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, ReviewDTO dto)
        {
            var review = _mapper.ToModel(dto, new Review());

            if (id != review.Id)
                return BadRequest();

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
                    return NotFound();
                
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<ReviewDTO>> PostReview(ReviewDTO dto)
        {
            var review = _mapper.ToModel(dto, new Review());

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReview", new {id = review.Id}, _mapper.ToDTO(review, new ReviewDTO()));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ReviewDTO>> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            
            if (review == null)
                return NotFound();

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return _mapper.ToDTO(review, new ReviewDTO());
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
