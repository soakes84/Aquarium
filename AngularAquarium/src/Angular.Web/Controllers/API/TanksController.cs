using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Aquarium.Data;
using Microsoft.AspNetCore.Authorization;
using Angular.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;


namespace Angular.Web.Controllers.Controllers.API
{
    [Produces("application/json")]
    [Authorize]
    public class TanksController : Controller
    {
        private readonly AquariumContext _context;
        private UserManager<ApplicationUser> _userManager { get; set; }

        public TanksController(UserManager<ApplicationUser> userManager, AquariumContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Route("~/tanks")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("~/tanks/{id}")]
        public IActionResult Tank(int id)
        {
            // var tank = Context.Tanks.Find(id);
            return View(model: id);
        }

        [Route("~/tanks/{tankId}/fishes/{id}")]
        public IActionResult Fish(int tankId, int id)
        {
            var fish = _context.Fishes.Find(id);

            return View(fish);
        }

        [Route("~/tanks/{id}/fishadd")]
        public IActionResult FishAdd(int id)
        {
            return View(model: id);
        }


        [HttpGet]
        [Route("~/api/tanks")]
        public IEnumerable<Tank> GetTanks()              
        {
            var userId = _userManager.GetUserId(User);
            return _context.Tanks.Where(q => q.OwnerId == userId).ToList();
        }

        [HttpGet]
        [Route("~/api/tanks/{id}")]
        public async Task<IActionResult> GetTank(int id)            
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = _userManager.GetUserId(User);
            Tank tank = await _context.Tanks
                .SingleOrDefaultAsync(p => p.OwnerId == userId && p.Id == id);

            if (tank == null)
            {
                return NotFound();
            }

            return Ok(tank);
        }

        [HttpPut]
        [Route("~/api/tanks/{id}")]
        public async Task<IActionResult> PutTank(int id, [FromBody] Tank tank)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tank.Id)
            {
                return BadRequest();
            }

            tank.Owner = await _userManager.GetUserAsync(User);
            _context.Entry(tank).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TankExists(id))
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

        [HttpPost]
        [Route("~/api/tanks")]
        public async Task<IActionResult> PostTank([FromBody] Tank tank)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            tank.Owner = await _userManager.GetUserAsync(User);
            _context.Tanks.Add(tank);
            

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTank", new { id = tank.Id }, tank);
        }

        [HttpDelete]
        [Route("~/api/tanks/{id}")]
        public async Task<IActionResult> DeleteTank(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = _userManager.GetUserId(User);

            Tank tank = await _context.Tanks
                .Where(q => q.OwnerId == userId)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (tank == null)
            {
                return NotFound();
            }

            _context.Tanks.Remove(tank);
            await _context.SaveChangesAsync();

            return Ok(tank);
        }


        private bool TankExists(int id)
        {
            var userId = _userManager.GetUserId(User);
            return _context.Tanks.Any(e => e.OwnerId == userId && e.Id == id);
        }


    }
}
