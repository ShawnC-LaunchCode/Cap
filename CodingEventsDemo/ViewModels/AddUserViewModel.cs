using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette_Identity.ViewModels
{
    public class AddUserViewModel
    {
        public string SSN { get; set; }

        public string PreferedDisplayName { get; set; }

        public AddUserViewModel()
        {
        }

        public AddUserViewModel(string userID)
        {
            SSN = userID;
        }


    }
}
