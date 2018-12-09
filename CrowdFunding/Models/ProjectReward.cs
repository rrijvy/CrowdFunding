using System.ComponentModel.DataAnnotations;

namespace CrowdFunding.Models
{
    public class ProjectReward
    {
        public int Id { get; set; }

        [Display(Name = "Reward description")]
        public string RewardDescription { get; set; }


        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
