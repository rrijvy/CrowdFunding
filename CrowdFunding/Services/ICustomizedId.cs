using CrowdFunding.Models;
using CrowdFunding.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFunding.Services
{
    public interface ICustomizedId
    {
        string EntreprenuerCustomId(Entrepreneur model);
        string InvestorCustomId(Investor model);
        string InvestmentRegNo(InvestmentViewModel model, string userId);
    }
}
