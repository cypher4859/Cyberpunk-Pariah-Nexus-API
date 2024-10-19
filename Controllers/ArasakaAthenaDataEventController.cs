using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CyberpunkPariahNexusApi.Models;
using CyberpunkPariahNexusApi.Models.Arasaka;

namespace CyberpunkPariahNexusApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArasakaAthenaDataEventController : ControllerBase
    {
        private readonly DataContext _context;

        public ArasakaAthenaDataEventController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ArasakaAthenaDataEvent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArasakaAthenaDataEvent>>> GetarasakaDataEvents()
        {
            return await _context.arasakaDataEvents.ToListAsync();
        }

        // GET: api/ArasakaAthenaDataEvent/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArasakaAthenaDataEvent>> GetArasakaAthenaDataEvent(int id)
        {
            var arasakaAthenaDataEvent = await _context.arasakaDataEvents.FindAsync(id);

            if (arasakaAthenaDataEvent == null)
            {
                return NotFound();
            }

            return arasakaAthenaDataEvent;
        }

        // PUT: api/ArasakaAthenaDataEvent/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArasakaAthenaDataEvent(int id, ArasakaAthenaDataEvent arasakaAthenaDataEvent)
        {
            if (id != arasakaAthenaDataEvent.id)
            {
                return BadRequest();
            }

            _context.Entry(arasakaAthenaDataEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArasakaAthenaDataEventExists(id))
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

        // POST: api/ArasakaAthenaDataEvent
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ArasakaAthenaDataEvent>> PostArasakaAthenaDataEvent(ArasakaAthenaDataEvent arasakaAthenaDataEvent)
        {
            _context.arasakaDataEvents.Add(arasakaAthenaDataEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArasakaAthenaDataEvent", new { id = arasakaAthenaDataEvent.id }, arasakaAthenaDataEvent);
        }

        // DELETE: api/ArasakaAthenaDataEvent/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArasakaAthenaDataEvent(int id)
        {
            var arasakaAthenaDataEvent = await _context.arasakaDataEvents.FindAsync(id);
            if (arasakaAthenaDataEvent == null)
            {
                return NotFound();
            }

            _context.arasakaDataEvents.Remove(arasakaAthenaDataEvent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArasakaAthenaDataEventExists(int id)
        {
            return _context.arasakaDataEvents.Any(e => e.id == id);
        }
    }
}
