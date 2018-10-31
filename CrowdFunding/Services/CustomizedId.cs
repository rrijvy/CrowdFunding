using System;
using System.Linq;
using CrowdFunding.Data;
using CrowdFunding.Models;
using CrowdFunding.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CrowdFunding.Services
{
    public class CustomizedId : ICustomizedId
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomizedId(ApplicationDbContext context,
                            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        public string InvestmentRegNo(InvestmentViewModel model, string userId)
        {
            string regNo = string.Empty;
            var user = _userManager.FindByIdAsync(userId);
            regNo += model.Project.Id.ToString() + model.Investment.Id.ToString();
            return regNo;
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
