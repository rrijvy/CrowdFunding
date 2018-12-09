using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFunding.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string ProjectTitle { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string EntreprenuerName { get; set; }
        public double PledgedAmount { get; set; }
        public double Funded { get; set; }
        public double DaysLeft { get; set; }
        public string CompanyName { get; set; }
        public string CountryName { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public int TotalBacker { get; set; }
        public string VideoUrl { get; set; }
        public double Viewed { get; set; }
        public string EntreprenuerId { get; set; }
    }
}
