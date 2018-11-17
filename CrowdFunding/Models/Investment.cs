using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdFunding.Models
{
    public class Investment
    {
        public Investment()
        {
            IsApproved = false;
        }
        public int Id { get; set; }

        public string InvestmentRegNo { get; set; }

        public double Amount { get; set; }

        public int ProjectId { get; set; }

        public int InvestmentTypeId { get; set; }

        public string InvestorId { get; set; }

        public bool IsApproved { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        [ForeignKey("InvestmentTypeId")]
        public InvestmentType InvestmentType { get; set; }

        [ForeignKey("InvestorId")]
        public Investor Investor { get; set; }


    }
}
