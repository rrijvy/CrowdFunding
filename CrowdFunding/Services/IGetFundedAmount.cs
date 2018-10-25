using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFunding.Services
{
    public interface IGetFundedAmount
    {
        double FundedAmount(int projectId);
    }
}
