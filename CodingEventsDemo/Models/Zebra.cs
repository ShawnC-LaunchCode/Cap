using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Roulette_Identity.Models
{
    public class User
    {
        public string Username { get; set; }
        public int Bank { get; set; }

        public User()
        {

        }

        public User(string name, int bank)
        {
            Username = name;
            Bank = bank;
        }


    }
}
