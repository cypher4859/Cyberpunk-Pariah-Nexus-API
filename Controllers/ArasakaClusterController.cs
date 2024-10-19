using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CyberpunkPariahNexusApi.Models;
using CyberpunkPariahNexusApi.Models.Arasaka;
using CyberpunkPariahNexusApi.Models.Arasaka.DTOs;

namespace CyberpunkPariahNexusApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArasakaClusterController : ControllerBase
    {
        private readonly DataContext _context;

        public ArasakaClusterController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ArasakaCluster
        // This is lazy-loaded since there could be a bunch 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArasakaCluster>>> GetarasakaClusters()
        {
            return await _context.arasakaClusters.ToListAsync();
        }

        // GET: api/ArasakaCluster/5
        // Supports EagerLoading
        [HttpGet("{id}")]
        public async Task<ActionResult<ArasakaCluster>> GetArasakaCluster(int id)
        {
            // var arasakaCluster = await _context.arasakaClusters.FindAsync(id);
            var arasakaCluster = await _context.arasakaClusters
                                                .Include(cluster => cluster.devices)
                                                .ThenInclude(device => device.processes)
                                                .Include(cluster => cluster.devices)
                                                .ThenInclude(device => device.memoryMappings)
                                                .FirstOrDefaultAsync(c => c.id == id); // Using this because it supports EagerLoading

            if (arasakaCluster == null)
            {
                return NotFound();
            }

            return arasakaCluster;
        }

        // PUT: api/ArasakaCluster/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArasakaCluster(int id, ArasakaCluster arasakaCluster)
        {
            if (id != arasakaCluster.id)
            {
                return BadRequest();
            }

            _context.Entry(arasakaCluster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArasakaClusterExists(id))
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

        // POST: api/ArasakaCluster
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ArasakaCluster>> PostArasakaCluster(ArasakaClusterDto clusterDto)
        {
            if (clusterDto == null) {
                return BadRequest("Cluster data is missing");
            }

            // Map the cluster DTO to the db model
            var arasakaCluster = new ArasakaCluster
            {
                clusterName = clusterDto.clusterName,
                nodeCount = clusterDto.nodeCount,
                cpuCores = clusterDto.cpuCores,
                memoryGb = clusterDto.memoryGb,
                storageTb = clusterDto.storageTb,
                creationDate = clusterDto.creationDate,
                environment = clusterDto.environment,
                kubernetesVersion = clusterDto.kubernetesVersion,
                region = clusterDto.region,
                devices = clusterDto.devices?.Select(d => new ArasakaDevice
                {
                    name = d.name,
                    architecture = d.architecture,
                    processorType = d.processorType,
                    region = d.region,
                    athenaAccessKey = d.athenaAccessKey,
                }).ToList()
            };

            // Save the cluster
            _context.arasakaClusters.Add(arasakaCluster);
            await _context.SaveChangesAsync();

            // then check and save devices
            if (arasakaCluster.devices != null && arasakaCluster.devices.Any()) {
                foreach (var device in arasakaCluster.devices) {
                    var existingDevice = await _context.arasakaDevices
                        .FirstOrDefaultAsync(d => d.name == device.name && d.region == device.region);

                    if (existingDevice == null) {
                        device.clusterId = arasakaCluster.id;
                        _context.arasakaDevices.Add(device);
                    } else {
                        existingDevice.clusterId = arasakaCluster.id;
                    }

                }
                await _context.SaveChangesAsync();
                
                // also setup a default process for each device 
                foreach (var device in arasakaCluster.devices) {
                    if (device.processes == null || !device.processes.Any()) {
                        var defaultProcess = new ArasakaDeviceProcess
                        {
                            memory = "4GB",
                            family = "DefaultFamily",
                            openFiles = "/var/log/system.log",
                            deviceId = device.id // Use the generated deviceId
                        };

                        _context.arasakaDeviceProcesses.Add(defaultProcess);
                    }

                }
                await _context.SaveChangesAsync();

                foreach (var device in arasakaCluster.devices) {
                    if (device.memoryMappings == null || !device.memoryMappings.Any()) {
                        var defaultMemoryMapping = new ArasakaDeviceMemoryMapping
                        {
                            memoryType = "DDR4",                  // Common memory type
                            memorySizeGb = 16,                    // Defaulting to 16 GB
                            memorySpeedMhz = 3200,                // 3200 MHz is a common speed for DDR4
                            memoryTechnology = "SDRAM",           // SDRAM (Synchronous DRAM)
                            memoryLatency = 16,                   // CAS Latency of 16 (common for DDR4 3200)
                            memoryVoltage = 1.35f,                // 1.35V is common for DDR4 high-performance RAM
                            memoryFormFactor = "DIMM",            // DIMM for desktop RAM, SODIMM for laptop
                            memoryEccSupport = 0,                 // 0: No ECC support, 1: ECC supported (typical consumer RAM doesn't support ECC)
                            memoryHeatSpreader = 1,               // Assume 1: Heat spreader is present
                            memoryWarrantyYears = 10,
                            deviceId = device.id
                        };
                        _context.arasakaDevicesMemoryMappings.Add(defaultMemoryMapping);
                    }
                }
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetArasakaCluster", new { id = arasakaCluster.id }, arasakaCluster);
        }

        // DELETE: api/ArasakaCluster/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArasakaCluster(int id)
        {
            var arasakaCluster = await _context.arasakaClusters.FindAsync(id);
            if (arasakaCluster == null)
            {
                return NotFound();
            }

            _context.arasakaClusters.Remove(arasakaCluster);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArasakaClusterExists(int id)
        {
            return _context.arasakaClusters.Any(e => e.id == id);
        }
    }
}
