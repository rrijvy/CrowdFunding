using CrowdFunding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFunding.ViewModels
{
    public class LovedProjectsViewModel
    {
        public int ProjectId { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string ProjectTitle { get; set; }
        public string ShortDescription { get; set; }
        public double NeededFund { get; set; }

        public int FavouriteId { get; set; }

        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
    }
}
