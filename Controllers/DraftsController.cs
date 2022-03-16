using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SjxLogistics.Data;
using SjxLogistics.Models.DatabaseModels;
using SjxLogistics.Models.Request;
using SjxLogistics.Models.Responses;

namespace SjxLogistics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DraftsController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public DraftsController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Drafts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drafts>>> GetDrafts()
        {
            return await _context.Drafts.ToListAsync();
        }

        // GET: api/Drafts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Drafts>> GetDrafts(int id)
        {
            var drafts = await _context.Drafts.FindAsync(id);

            if (drafts == null)
            {
                return NotFound();
            }

            return drafts;
        }

        // PUT: api/Drafts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDrafts(int id, Drafts drafts)
        {
            if (id != drafts.id)
            {
                return BadRequest();
            }

            _context.Entry(drafts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DraftsExists(id))
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

        // POST: api/Drafts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Drafts>> PostDrafts([FromBody] DraftDetails draft)
        {
            var response = new ServiceResponses<Drafts>();
            string rawUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (!int.TryParse(rawUser, out int userId))
                return Unauthorized();

            var user = await _context.Users.FindAsync(userId);

            try
            {
                Drafts drafts = new()
                {
                    StartAddress = draft.StartAddress,
                    EndAddress = draft.EndAddress,
                    weight = draft.weight,
                    Categories = draft.Categories,
                    price = draft.price
                };

                _context.Drafts.Add(drafts);
                await _context.SaveChangesAsync();
                response.Messages = "Successful";
                response.StatusCode = 200;
                response.Data = drafts;
                response.Success = true;

                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE: api/Drafts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDrafts(int id)
        {
            var drafts = await _context.Drafts.FindAsync(id);
            if (drafts == null)
            {
                return NotFound();
            }

            _context.Drafts.Remove(drafts);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DraftsExists(int id)
        {
            return _context.Drafts.Any(e => e.id == id);
        }
    }
}
