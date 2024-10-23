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
    public class ArasakaDeviceController : ControllerBase
    {
        private readonly DataContext _context;

        public ArasakaDeviceController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ArasakaDevice
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArasakaDevice>>> GetarasakaDevices()
        {
            return await _context.arasakaDevices.ToListAsync();
        }

        // GET: api/ArasakaDevice/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArasakaDevice>> GetArasakaDevice(int id)
        {
            var arasakaDevice = await _context.arasakaDevices
                                                .Include(d => d.processes)
                                                .Include(d => d.memoryMappings)
                                                .Include(d => d.dataEvents)
                                                .FirstOrDefaultAsync(d => d.id == id);

            if (arasakaDevice == null)
            {
                return NotFound();
            }

            return arasakaDevice;
        }

        // PUT: api/ArasakaDevice/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutArasakaDevice(int id, ArasakaDevice arasakaDevice)
        // {
        //     if (id != arasakaDevice.id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(arasakaDevice).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!ArasakaDeviceExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

        // // POST: api/ArasakaDevice
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<ArasakaDevice>> PostArasakaDevice(ArasakaDevice arasakaDevice)
        // {
        //     _context.arasakaDevices.Add(arasakaDevice);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction("GetArasakaDevice", new { id = arasakaDevice.id }, arasakaDevice);
        // }

        // // DELETE: api/ArasakaDevice/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteArasakaDevice(int id)
        // {
        //     var arasakaDevice = await _context.arasakaDevices.FindAsync(id);
        //     if (arasakaDevice == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.arasakaDevices.Remove(arasakaDevice);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        private bool ArasakaDeviceExists(int id)
        {
            return _context.arasakaDevices.Any(e => e.id == id);
        }
    }
}
