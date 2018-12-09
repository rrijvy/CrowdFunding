using CrowdFunding.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrowdFunding.Models
{
    public class Country
    {
        public int Id { get; set; }

        [Display(Name = "Country name")]
        public string Name { get; set; }

        public List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
