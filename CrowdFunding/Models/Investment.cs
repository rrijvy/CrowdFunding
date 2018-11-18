using System.ComponentModel.DataAnnotations;
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

        [Display(Name = "Registration number")]
        public string InvestmentRegNo { get; set; }

        public double Amount { get; set; }

        [Display(Name ="Project name")]
        public int ProjectId { get; set; }

        [Display(Name = "Investment type")]
        public int InvestmentTypeId { get; set; }

        [Display(Name = "Investor name")]
        public string InvestorId { get; set; }

        [Display(Name = "Is approved")]
        public bool IsApproved { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        [ForeignKey("InvestmentTypeId")]
        public InvestmentType InvestmentType { get; set; }

        [ForeignKey("InvestorId")]
        public Investor Investor { get; set; }


    }
}
