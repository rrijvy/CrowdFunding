using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdFunding.Models
{
    public class Funded
    {
        public int Id { get; set; }

        public int InvestmentId { get; set; }

        public int ProjectId { get; set; }

        public string InvestorId { get; set; }

        public double Amount { get; set; }

        public double RaisedAmount { get; set; }

        public bool IsLive { get; set; }

        

        [ForeignKey("InvestmentId")]
        public Investment Investment { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        [ForeignKey("InvestorId")]
        public Investor Investor { get; set; }


    }
}
