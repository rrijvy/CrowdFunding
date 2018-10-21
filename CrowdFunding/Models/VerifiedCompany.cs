using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdFunding.Models
{
    public class VerifiedCompany
    {
        public int Id { get; set; }

        [ForeignKey("CompanyId")]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

    }
}
