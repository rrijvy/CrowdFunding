using CrowdFunding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFunding.ViewModels
{
    public class InvestorDashboardViewModel
    {
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string PermanentAddress { get; set; }
        public string PresentAddress { get; set; }
        public string Country { get; set; }
        public List<Project> BackedProject { get; set; }
    }
}
