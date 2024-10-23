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
    public class ArasakaDeviceMemoryMappingController : ControllerBase
    {
        private readonly DataContext _context;

        public ArasakaDeviceMemoryMappingController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ArasakaDeviceMemoryMapping
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArasakaDeviceMemoryMapping>>> GetarasakaDevicesMemoryMappings()
        {
            return await _context.arasakaDevicesMemoryMappings.ToListAsync();
        }

        // GET: api/ArasakaDeviceMemoryMapping/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArasakaDeviceMemoryMapping>> GetArasakaDeviceMemoryMapping(int id)
        {
            var arasakaDeviceMemoryMapping = await _context.arasakaDevicesMemoryMappings.FindAsync(id);

            if (arasakaDeviceMemoryMapping == null)
            {
                return NotFound();
            }

            return arasakaDeviceMemoryMapping;
        }

        // PUT: api/ArasakaDeviceMemoryMapping/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutArasakaDeviceMemoryMapping(int id, ArasakaDeviceMemoryMapping arasakaDeviceMemoryMapping)
        // {
        //     if (id != arasakaDeviceMemoryMapping.id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(arasakaDeviceMemoryMapping).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!ArasakaDeviceMemoryMappingExists(id))
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

        // // POST: api/ArasakaDeviceMemoryMapping
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<ArasakaDeviceMemoryMapping>> PostArasakaDeviceMemoryMapping(ArasakaDeviceMemoryMapping arasakaDeviceMemoryMapping)
        // {
        //     _context.arasakaDevicesMemoryMappings.Add(arasakaDeviceMemoryMapping);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction("GetArasakaDeviceMemoryMapping", new { id = arasakaDeviceMemoryMapping.id }, arasakaDeviceMemoryMapping);
        // }

        // // DELETE: api/ArasakaDeviceMemoryMapping/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteArasakaDeviceMemoryMapping(int id)
        // {
        //     var arasakaDeviceMemoryMapping = await _context.arasakaDevicesMemoryMappings.FindAsync(id);
        //     if (arasakaDeviceMemoryMapping == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.arasakaDevicesMemoryMappings.Remove(arasakaDeviceMemoryMapping);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        private bool ArasakaDeviceMemoryMappingExists(int id)
        {
            return _context.arasakaDevicesMemoryMappings.Any(e => e.id == id);
        }
    }
}
