using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
