using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFunding.Models
{
    public class ProjectReward
    {
        public int Id { get; set; }
        public string RewardDescription { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
