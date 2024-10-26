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
    public class MikoshiController : ControllerBase
    {
        private readonly DataContext _context;

        public MikoshiController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Mikoshi
        [HttpGet]
        public async Task<ActionResult<string>> GetMikoshi(string adminKey)
        {
            if (!AuthorizationService.HandleAdminAuthorization(adminKey)) {
                return Unauthorized();
            }
            return "You won! Great Job Net Runner!";
        }
    }
}
