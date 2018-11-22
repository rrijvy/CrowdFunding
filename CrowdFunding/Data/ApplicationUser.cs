using CrowdFunding.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdFunding.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name ="National ID no.")]
        public string NID { get; set; }

        [Display(Name = "First name")]
        public string FName { get; set; }

        [Display(Name = "Last name")]
        public string LName { get; set; }

        [Display(Name = "Date of birth"), DataType(DataType.Date)]
        public DateTime DateofBirth { get; set; }

        [Display(Name = "Present address")]
        public string PresentAddress { get; set; }

        [Display(Name = "Parmanent address")]
        public string ParmanantAddress { get; set; }

        public string Image { get; set; }

        [Display(Name = "Country name")]
        public int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country Country { get; set; }

        public List<Favourite> Favourites{ get; set; }
    }
}
