using System.Collections.Generic;

namespace CrowdFunding.Models
{
    public class ProjectCategory
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public List<Project> Projects { get; set; }
    }
}
