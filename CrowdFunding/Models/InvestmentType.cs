using System.Collections.Generic;

namespace CrowdFunding.Models
{
    public class InvestmentType
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public List<Investment> Investments { get; set; }
    }
}
