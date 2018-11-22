using CrowdFunding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFunding.ViewModels
{
    public class HomeIndexVIewModel
    {
        public List<ProjectCategory> ProjectCategories { get; set; }
        public List<Project> Projects { get; set; }
        public List<Project> FileProjects { get; set; }
        public Project LastProject { get; set; }
    }
}
