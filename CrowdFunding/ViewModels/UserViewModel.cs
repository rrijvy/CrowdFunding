using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFunding.ViewModels
{
    public class UserViewModel
    {
        public string Name { get; set; }
        public string About { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        public int Companies { get; set; }
        public string Website { get; set; }
        public string Avater { get; set; }
        public string Country { get; set; }
        public int Backed { get; set; }
    }
}
