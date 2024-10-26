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
    public class ArasakaDeviceProcessController : ControllerBase
    {
        private readonly DataContext _context;

        public ArasakaDeviceProcessController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ArasakaDeviceProcess
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArasakaDeviceProcess>>> GetarasakaDeviceProcesses()
        {
            return await _context.arasakaDeviceProcesses.ToListAsync();
        }

        // GET: api/ArasakaDeviceProcess/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArasakaDeviceProcess>> GetArasakaDeviceProcess(int id)
        {
            var arasakaDeviceProcess = await _context.arasakaDeviceProcesses.FindAsync(id);

            if (arasakaDeviceProcess == null)
            {
                return NotFound();
            }

            return arasakaDeviceProcess;
        }

        // PUT: api/ArasakaDeviceProcess/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArasakaDeviceProcess(int id, ArasakaDeviceProcess arasakaDeviceProcess)
        {
            if (id != arasakaDeviceProcess.id)
            {
                return BadRequest();
            }

            _context.Entry(arasakaDeviceProcess).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArasakaDeviceProcessExists(id))
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

        // POST: api/ArasakaDeviceProcess
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ArasakaDeviceProcess>> PostArasakaDeviceProcess(ArasakaDeviceProcess arasakaDeviceProcess)
        {
            _context.arasakaDeviceProcesses.Add(arasakaDeviceProcess);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArasakaDeviceProcess", new { id = arasakaDeviceProcess.id }, arasakaDeviceProcess);
        }

        // DELETE: api/ArasakaDeviceProcess/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArasakaDeviceProcess(int id)
        {
            var arasakaDeviceProcess = await _context.arasakaDeviceProcesses.FindAsync(id);
            if (arasakaDeviceProcess == null)
            {
                return NotFound();
            }

            _context.arasakaDeviceProcesses.Remove(arasakaDeviceProcess);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArasakaDeviceProcessExists(int id)
        {
            return _context.arasakaDeviceProcesses.Any(e => e.id == id);
        }
    }
}
