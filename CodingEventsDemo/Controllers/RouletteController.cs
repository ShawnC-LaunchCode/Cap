﻿using System;
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
using Microsoft.AspNetCore.Authorization;

namespace Roulette_Identity.Controllers
{
    [Authorize]
    public class RouletteController : Controller

    {
        private RouletteDbContext context;
        private readonly UserManager<IdentityUser> _userManager;

        //these can be stored in context
        private static List<Bet> bets = new List<Bet>();
        private static int LastSpin = 0;
        private static Zebra user;
       




        public RouletteController(RouletteDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            
            context = dbContext;
            _userManager = userManager;

            //string userId = _userManager.GetUserId(User);

            //List<Zebra> list = context.Zebras//which zebra has right ssn? pull current data from persistant
            //   //.Include(z => z.SSN)
            //   .Where(z => z.SSN == userId)
            //   .ToList();

            //user = list[0];


        }

        public IActionResult Index()//everything to display to user 
        {
            string userId = _userManager.GetUserId(User);

            List<Zebra> list = context.Zebras//which zebra has right ssn? pull current data from persistant
                //.Include(z => z.SSN)
                .Where(z => z.SSN == userId)
                .ToList();

            


            if (list.Count() == 0)
            {
                return Redirect("/User/Add");
            }

            user = list[0];

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
            string userId = _userManager.GetUserId(User);

            List<Zebra> list = context.Zebras//which zebra has right ssn? pull current data from persistant
                //.Include(z => z.SSN)
                .Where(z => z.SSN == userId)
                .ToList();

            user = list[0];

            if (betAmount <= user.Bank)
            {
                context.Zebras.Find(user.Id).Bank -= betAmount;
                context.SaveChanges();
                //user.Bank -= betAmount;
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
            string userId = _userManager.GetUserId(User);

            List<Zebra> list = context.Zebras//which zebra has right ssn? pull current data from persistant
                                             //.Include(z => z.SSN)
                .Where(z => z.SSN == userId)
                .ToList();

            user = list[0];


            foreach (Bet bet in bets)
            {
                context.Zebras.Find(user.Id).Bank += bet.Amount;
                context.SaveChanges();
                //user.Bank += bet.Amount;
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
            context.Zebras.Find(user.Id).Bank = 5000;
            context.SaveChanges();
            //user.Bank = 5000;
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

                switch (betCatagory) //switch to be ready for adding outside bets
                {
                    case 0://inside bets

                        if (betType == LastSpin)
                        {
                            context.Zebras.Find(user.Id).Bank += bet.Amount * 35;
                            context.SaveChanges();
                            //user.Bank += bet.Amount * 35;
                        }
                        
                        break;

                    default:
                        break;
                }

            }
        }
    }
}
