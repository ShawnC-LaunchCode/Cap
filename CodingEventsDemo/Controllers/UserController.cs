using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Roulette_Identity.Data;

namespace Roulette_Identity.Controllers
{
    public class UserController : Controller
    {
        private RouletteDbContext context;
        private readonly UserManager<IdentityUser> _userManager;
        public IActionResult Index() 
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        public UserController(RouletteDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            context = dbContext;
            _userManager = userManager;
        }

    }
}
