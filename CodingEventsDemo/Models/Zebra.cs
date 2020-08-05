using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Roulette_Identity.Models
{
    public class Zebra
    { 
        public int Id { get; set; }  //ZebraId in viewmodels
        public string SSN {get; set; }
        public string Username { get; set; }
        public int Bank { get; set; }

        public int UserLevel { get; set; } //0=bot, 1=user, 2=admin

        public Zebra()
        {

        }

        public Zebra(string name, int bank)
        {
            Username = name;
            Bank = bank;
        }
    }
}
