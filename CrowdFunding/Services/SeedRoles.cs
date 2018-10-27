using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFunding.Services
{
    public class SeedRoles
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async void Seed()
        {
            string[] roles = { "Admin", "Entreprenuer", "Investor", "Moderator" };

            foreach (string item in roles)
            {
                if (await _roleManager.FindByNameAsync(item) == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = item });
                }                
            }
        }
    }
}
