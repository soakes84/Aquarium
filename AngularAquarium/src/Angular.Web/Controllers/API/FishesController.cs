using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Aquarium.Data;
using Aquarium.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Angular.Web.Models;


namespace Aquarium.Controllers
{
    [Produces("application/json")]
    [Authorize]
    public class FishesController : Controller
    {
        private readonly AquariumContext _context;
        private UserManager<ApplicationUser> _userManager { get; set; }

        public FishesController(UserManager<ApplicationUser> userManager, AquariumContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Route("~/tanks/{tankId}/fishes")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("~/api/fishes")]
        public IEnumerable<Fish> GetFish()
        {
            var userId = _userManager.GetUserId(User);
            return _context.Fishes.Where(q => q.OwnerId == userId).ToList();
        }

        [HttpGet]
        [Route("~/api/tanks/{id}/fishes")]
        public async Task<IActionResult> GetFish(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = _userManager.GetUserId(User);
            var fish = await _context.Fishes
                .Where(p => p.TankId == id).ToListAsync();

            if (fish == null)
            {
                return NotFound();
            }

            return Ok(fish);
        }

        [HttpGet]
        [Route("~/api/tanks/{tankId}/fishes/{fishId}")]
        public async Task<IActionResult> GetSingleFish(int tankId, int fishId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = _userManager.GetUserId(User);
            var fish = await _context.Fishes
                .FirstOrDefaultAsync(q => q.Id == fishId);

            if(fish == null)
            {
                return NotFound();
            }

            return Ok(fish);
        }

        [HttpPut]
        [Route("~/api/fishes/{id}")]
        public async Task<IActionResult> PutFish(int id, [FromBody] Fish fish)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fish.Id)
            {
                return BadRequest();
            }


            fish.Owner = await _userManager.GetUserAsync(User);
            _context.Entry(fish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FishExists(id))
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
        [Route("~/api/tanks/{tankId}/fishes")]
        public async Task<IActionResult> PostFish(int tankId, [FromBody] Fish fish)
        {
            var tank = _context.Tanks.FirstOrDefault(q => q.Id == tankId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            fish.Owner = await _userManager.GetUserAsync(User);
            fish.Tank = tank;

            _context.Fishes.Add(fish);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FishExists(fish.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFish", new { id = fish.Id }, fish);
        }

        [HttpPost]
        [Route("~/api/tanks/{tankId}")]
        public async Task<IActionResult> PostShark(int tankId, [FromBody] Fish fish)
        {
            var tank = _context.Tanks.FirstOrDefault(q => q.Id == tankId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            fish.Owner = await _userManager.GetUserAsync(User);
            fish.Tank = tank;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FishExists(fish.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFish", new { id = fish.Id }, fish);
        }

        [HttpDelete]
        [Route("~/api/fishes/{id}")]
        public async Task<IActionResult> DeleteFish(int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = _userManager.GetUserId(User);

            Fish fish = await _context.Fishes
                .Where(q => q.OwnerId == userId)
                .SingleOrDefaultAsync(m => m.Id == id);

            if(fish == null)
            {
                return NotFound();
            }

            _context.Fishes.Remove(fish);
            await _context.SaveChangesAsync();

            return Ok(fish);
        }

        private bool FishExists(int id)
        {
            var userId = _userManager.GetUserId(User);
            return _context.Fishes.Any(e => e.OwnerId == userId && e.Id == id);
        }


    }
}