using Roulette_Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette_Identity.ViewModels
{
    public class RouletteViewModel
    {

        //name-bank-BET-LIST(bets)

        public List<Bet> Bets { get; set; }

        public int BetAmount { get; set; }

        public User Player { get; set; }

        public int LastSpinNumber { get; set; }

        public List<int> History { get; set; }

    }
}
