using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SjxLogistics.Data;
using SjxLogistics.Models.DatabaseModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SjxLogistics.Controllers.RiderController
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikesController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public BikesController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Bikes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bikes>>> GetBikes()
        {
            return await _context.Bikes.ToListAsync();
        }

        // GET: api/Bikes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bikes>> GetBikes(int id)
        {
            var bikes = await _context.Bikes.FindAsync(id);

            if (bikes == null)
            {
                return NotFound();
            }

            return bikes;
        }

        // PUT: api/Bikes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBikes(int id, Bikes bikes)
        {
            if (id != bikes.id)
            {
                return BadRequest();
            }

            _context.Entry(bikes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BikesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Bikes
        // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Bikes>> PostBikes(Bikes bikes)
        {
            _context.Bikes.Add(bikes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBikes", new { bikes.id }, bikes);
        }

        // DELETE: api/Bikes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBikes(int id)
        {
            var bikes = await _context.Bikes.FindAsync(id);
            if (bikes == null)
            {
                return NotFound();
            }

            _context.Bikes.Remove(bikes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BikesExists(int id)
        {
            return _context.Bikes.Any(e => e.id == id);
        }
    }
}
