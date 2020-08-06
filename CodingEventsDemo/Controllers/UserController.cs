using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Roulette_Identity.Data;
using Roulette_Identity.Models;
using Roulette_Identity.ViewModels;

namespace Roulette_Identity.Controllers
{
    public class UserController : Controller
    {
        private RouletteDbContext context;
        private readonly UserManager<IdentityUser> _userManager;
        private Zebra user;
        public IActionResult Index() 
        {
            string userId = _userManager.GetUserId(User);

            List<Zebra> list = context.Zebras//which zebra has right ssn? pull current data from persistant
                                             //.Include(z => z.SSN)
                .Where(z => z.SSN == userId)
                .ToList();


            if (list.Count() == 0)//identity user created, but no zebra
            {
                AddUserViewModel viewModel = new AddUserViewModel();
                return View("/User/Add", viewModel);
            }


            List<Zebra> viewList = context.Zebras.ToList();
                







            return View(viewList);
        }

        public IActionResult Add()
        {
            string userId = _userManager.GetUserId(User);

            //List<Zebra> list = context.Zebras//which zebra has right ssn? pull current data from persistant
            //                                 //.Include(z => z.SSN)
            //    .Where(z => z.SSN == userId)
            //    .ToList();
            //user = list[0];

            AddUserViewModel viewModel = new AddUserViewModel
            {
                SSN = userId,

            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Add(AddUserViewModel addUserViewModel)
        {
            context.Zebras.Add(new Zebra
            {
                SSN = addUserViewModel.SSN,
                Username = addUserViewModel.Name,
                Bank = 5000,
                UserLevel=1
            });
            context.SaveChanges();
            return Redirect("/Roulette");
        }


        public IActionResult Edit(int zebraId)
        {
            Zebra toEdit = context.Zebras.Find(zebraId);

            return View(toEdit);
        }

        [HttpPost]
        public IActionResult Edit (Zebra editedZebra)
        {


            return Redirect("/User");
        }

        public UserController(RouletteDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            context = dbContext;
            _userManager = userManager;
        }

    }
}
