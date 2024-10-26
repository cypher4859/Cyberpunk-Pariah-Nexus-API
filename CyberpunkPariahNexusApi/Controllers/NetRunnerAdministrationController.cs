using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CyberpunkPariahNexusApi.Models;
using CyberpunkPariahNexusApi.Models.Arasaka;
using CyberpunkPariahNexusApi.Helpers;

namespace CyberpunkPariahNexusApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class NetRunnerAdministrationController : ControllerBase
    {
        private readonly DataContext _context;

        public NetRunnerAdministrationController(DataContext context)
        {
            _context = context;
        }

        // GET: api/NetRunnerAdministration
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NetRunnerAdministration>>> GetnetRunnerAdministrations(string adminKey)
        {
            if (!AuthorizationService.HandleAdminAuthorization(adminKey)) {
                return Unauthorized();
            }
            return await _context.netRunnerAdministrations.ToListAsync();
        }

        // GET: api/NetRunnerAdministration/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NetRunnerAdministration>> GetNetRunnerAdministration(int id, string adminKey)
        {
            if (!AuthorizationService.HandleAdminAuthorization(adminKey)) {
                return Unauthorized();
            }
            var netRunnerAdministration = await _context.netRunnerAdministrations.FindAsync(id);

            if (netRunnerAdministration == null)
            {
                return NotFound();
            }

            return netRunnerAdministration;
        }

        // POST: api/NetRunnerAdministration
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NetRunnerAdministration>> PostNetRunnerAdministration(NetRunnerAdministration netRunnerAdministration, string adminKey)
        {
            if (!AuthorizationService.HandleAdminAuthorization(adminKey)) {
                return Unauthorized();
            }
            _context.netRunnerAdministrations.Add(netRunnerAdministration);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNetRunnerAdministration", new { id = netRunnerAdministration.id }, netRunnerAdministration);
        }

        // DELETE: api/NetRunnerAdministration/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNetRunnerAdministration(int id, string adminKey)
        {
            if (!AuthorizationService.HandleAdminAuthorization(adminKey)) {
                return Unauthorized();
            }
            var netRunnerAdministration = await _context.netRunnerAdministrations.FindAsync(id);
            if (netRunnerAdministration == null)
            {
                return NotFound();
            }

            _context.netRunnerAdministrations.Remove(netRunnerAdministration);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NetRunnerAdministrationExists(int id)
        {
            return _context.netRunnerAdministrations.Any(e => e.id == id);
        }
    }
}
