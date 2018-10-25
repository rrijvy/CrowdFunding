using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFunding.Models
{
    public class CompanyType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public List<Company> Companies { get; set; }
    }
}
