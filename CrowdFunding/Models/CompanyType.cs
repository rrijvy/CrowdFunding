using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrowdFunding.Models
{
    public class CompanyType
    {
        public int Id { get; set; }

        [Display(Name = "Type name")]
        public string TypeName { get; set; }

        public List<Company> Companies { get; set; }
    }
}
