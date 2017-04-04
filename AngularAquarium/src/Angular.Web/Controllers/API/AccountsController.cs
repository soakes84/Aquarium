using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Aquarium.Data;
using Aquarium.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Aquarium.Controllers
{
    [Produces("application/json")]
    //[Route("api/accounts")]
    public class AccountsController : Controller
    {
        public SignInManager<ApplicationUser> SignInManager { get; set; }
        public UserManager<ApplicationUser> UserManager { get; set; }

        public AccountsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }


        [Route("~/accounts/login")]
        // GET: /<controller>/
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("~/api/accounts/login")]
        public async Task<IActionResult> Login([FromBody]LoginRequest model)
        {

            var user = await UserManager.FindByEmailAsync(model.Email);


            if (user != null)
            {
                var result = await SignInManager.PasswordSignInAsync(user, model.Password, false, true);
                return Ok();

            }
            else
            {
                return BadRequest();
            }
        }



        [Route("~/accounts/register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("~/api/accounts/register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequest model)
        {
            var user = new ApplicationUser();
            user.Email = user.UserName = model.Email;
            user.Signature = Guid.NewGuid();

            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpGet]
        [Route("~/accounts/logout")]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();

            return Redirect("~/");
        }

    }
}
