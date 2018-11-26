using CrowdFunding.Models;
using System.Collections.Generic;

namespace CrowdFunding.ViewModels
{
    public class HomeIndexVIewModel
    {
        public List<ProjectCategory> ProjectCategories { get; set; }
        public List<Project> Projects { get; set; }
        public List<Project> FileProjects { get; set; }
        public Project LastProject { get; set; }
        public List<Project> LatestProject { get; set; }
    }
}
