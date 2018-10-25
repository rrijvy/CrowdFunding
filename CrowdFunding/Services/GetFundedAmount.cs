using CrowdFunding.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFunding.Services
{
    public class GetFundedAmount : IGetFundedAmount
    {
        private readonly ApplicationDbContext _context;

        public GetFundedAmount(ApplicationDbContext context)
        {
            _context = context;
        }
        

        public double FundedAmount(int projectId)
        {
            var funds = _context.Fundeds.Where(x => x.Id == projectId).ToList();
            var totalFunded = funds.Select(x => x.Amount).Count();
            return totalFunded;
        }
    }
}
