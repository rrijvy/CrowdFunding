using CrowdFunding.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdFunding.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string NID { get; set; }

        public string FName { get; set; }

        public string LName { get; set; }

        [Display(Name = "Date of birth"), DataType(DataType.Date)]
        public DateTime DateofBirth { get; set; }

        public string PresentAddress { get; set; }

        public string ParmanantAddress { get; set; }

        public string Image { get; set; }

        public int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country Country { get; set; }
    }
}
