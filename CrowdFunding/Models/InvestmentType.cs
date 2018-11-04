using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdFunding.Models
{
    public class InvestmentType
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string ShortDescription { get; set; }

        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public List<Investment> Investments { get; set; }
    }
}
