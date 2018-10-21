using CrowdFunding.Data;
using System.Collections.Generic;

namespace CrowdFunding.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
