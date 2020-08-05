using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Roulette_Identity.Data;
using Roulette_Identity.ViewModels;
using Roulette_Identity.Models;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Roulette_Identity.Controllers
{
    public class RouletteController : Controller

    {
        private RouletteDbContext context;
        private readonly UserManager<IdentityUser> _userManager;

        //these can be stored in context
        private static List<Bet> bets = new List<Bet>();
        private static int LastSpin = 0;
        private static Zebra user = new Zebra("Shawn", 5000);



        public RouletteController(RouletteDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            context = dbContext;
            _userManager = userManager;
        }

        public IActionResult Index()//everything to display to user 
        {
            string userId = _userManager.GetUserId(User);
            //TODO generate name and bank from User


            //thisZebra=which zebra has this ssn?

            List<Zebra> list = context.Zebras
                //.Include(z => z.SSN)
                .Where(z => z.SSN == userId)
                .ToList();



            Console.WriteLine(list);

            if (list.Count() == 0)
            {
               

                return Redirect("/User");
            }






            //Zebra theUser = context.Zebras.ToLookup(z=> z.SSN == (_userManager.GetUserId(User)));

            //var specialZebra = from z in context.Zebras
            //                   join u in context.Users
            //                   on z.SSN equals u.UserName
            //                   where u.UserName == z.SSN
            //                   select z;
            //Console.WriteLine((Zebra)specialZebra);



            //if(theUser.Username==null){
            //    Redirect("/Users/Add");
            //}

            //go get zebra


            RouletteViewModel viewModel = new RouletteViewModel
            {
                UserId = userId,
                Bets = bets,
                BetAmount = 50,
                Player = list[0],
                LastSpinNumber = LastSpin,
                ZebraId=list[0].Id
            };
            return View(viewModel);
        }


        [HttpPost]
        public IActionResult PlaceBet(int betAmount, string betType)
        {
            if (betAmount <= user.Bank)
            {

                user.Bank -= betAmount;
                Bet bet = new Bet
                {
                    Amount = betAmount,
                    Type = betType
                };

                bets.Add(bet);
            }

            return Redirect("/Roulette");
        }

        [HttpPost]
        public IActionResult ResetBets()
        {
            foreach (Bet bet in bets)
            {
                user.Bank += bet.Amount;
            }


            bets.Clear();
            return Redirect("/Roulette");
        }

        [HttpPost]
        public IActionResult SpinWheel()
        {
            LastSpin = NewNumber();
            PayBets();
            bets.Clear();

            return Redirect("/Roulette");
        }

        [HttpPost]
        public IActionResult ResetBank()
        {
            user.Bank = 5000;
            bets.Clear();
            return Redirect("/Roulette");
        }

        private int NewNumber()
        {
            Random rnd = new Random();
            int next = rnd.Next(0, 37);
            return next;
        }

        private void PayBets()
        {
            foreach (Bet bet in bets)
            {
                int betType = Int16.Parse(bet.Type);
                int betCatagory; // 0== number bet(0-36),  37+==outside bets(evens, odds, high/low...)
                if (betType >= 0 && betType <= 36)
                {
                    betCatagory = 0;
                }
                else
                {
                    betCatagory = betType;
                }

                switch (betCatagory)
                {
                    case 0://inside bets

                        if (betType == LastSpin)
                        {
                            user.Bank += bet.Amount * 35;
                        }
                        break;

                    default:
                        break;
                }

            }
        }
    }
}
