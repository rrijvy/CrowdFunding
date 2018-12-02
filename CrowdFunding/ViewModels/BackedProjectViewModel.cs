using System;

namespace CrowdFunding.ViewModels
{
    public class BackedProjectViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime BackedDate { get; set; }
        public double BackedAmount { get; set; }
        public string ChoosenReward { get; set; }
        public string Image { get; set; }
        public string ProjectTitle { get; set; }
        public double TotalAmountBacked { get; set; }
    }
}
