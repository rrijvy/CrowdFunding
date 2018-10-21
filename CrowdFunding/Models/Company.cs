using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdFunding.Models
{
    public class Company
    {
        public int Id { get; set; }
        
        [Required, Display(Name ="Company name")]
        public string Name { get; set; }
        
        [Required, Display(Name ="Registration no.")]
        public string RegNo { get; set; }

        [Required, Display(Name ="Company type")]
        public int CompanyTypeId { get; set; }

        [Display(Name = "Entrepreneur name")]
        public string EntrepreneurId { get; set; }
       
        [Required, EmailAddress]
        public string Email { get; set; }

        public string Liesence { get; set; }

        [Display(Name = "Phone number"), Phone]
        public string PhoneNo { get; set; }

        public string Address { get; set; }

        public string WebsiteUrl { get; set; }

        public string Video { get; set; }        

        [ForeignKey("CompanyTypeId")]
        public CompanyType CompanyType { get; set; }

        [ForeignKey("EntrepreneurId")]
        public Entrepreneur Entrepreneur { get; set; }        

        public List<Project> Projects { get; set; }

    }
}
