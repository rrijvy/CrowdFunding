using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdFunding.Data;
using CrowdFunding.Models;

namespace CrowdFunding.Services
{
    public class CustomizedId : ICustomizedId
    {
        private ApplicationDbContext _context;

        public CustomizedId(ApplicationDbContext context)
        {
            _context = context;
        }
        public string EntreprenuerCustomId(Entrepreneur model)
        {
            string customId = string.Empty;
            string defaultValue = "EN";
            int year = DateTime.Now.Year;
            int id = _context.Entrepreneurs.Count() + 1;
            customId = defaultValue + year.ToString().Trim() + id.ToString().Trim().PadLeft(2, '0');
            return customId;
        }

        public string InvestorCustomId(Investor model)
        {
            string customId = string.Empty;
            string defaultValue = "IN";
            int year = DateTime.Now.Year;
            int id = _context.Investors.Count() + 1;
            customId = defaultValue + year.ToString().Trim() + id.ToString().Trim().PadLeft(2, '0');
            return customId;
        }
    }
}
