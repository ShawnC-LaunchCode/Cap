using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette_Identity.Models
{
    public class Bet
    {
        public int Id { get; set; }
        public int ZebraId { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }

        public Bet()
        {

        }
    }
}
