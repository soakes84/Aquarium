using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Aquarium.Data;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Angular.Web.Controllers     //Aquarium.Controllers
{
    public class HomeController : Controller
    {
        private AquariumContext Context { get; set; }

        public HomeController()
        {
            Context = new AquariumContext();
        }
       // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [Route("~/home/tankadd")]
        public IActionResult TankAdd()
        {
            return View();
        }

        [Route("~/home/tanks")]
        public IActionResult Tanks()
        {
            return View();
        }


    }
}
