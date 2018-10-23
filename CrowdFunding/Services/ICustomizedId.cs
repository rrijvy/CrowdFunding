using CrowdFunding.Models;
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
    }
}
