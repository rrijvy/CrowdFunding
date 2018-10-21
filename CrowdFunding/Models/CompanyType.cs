using System.Collections.Generic;

namespace CrowdFunding.Models
{
    public class CompanyType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }

        public List<Company> Companies { get; set; }
    }
}
