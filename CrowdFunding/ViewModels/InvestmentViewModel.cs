using CrowdFunding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFunding.ViewModels
{
    public class InvestmentViewModel
    {
        public Project Project { get; set; }
        public Investment Investment { get; set; }
        public List<InvestmentType> InvestmentTypes { get; set; }
    }
}
