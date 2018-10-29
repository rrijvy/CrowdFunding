using CrowdFunding.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFunding.Services
{
    public static class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context, IServiceProvider service)
        {
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "Entreprenuer", "Investor" };

            IdentityResult roles;
            foreach (var item in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(item);
                if (!roleExist)
                {
                    roles = await roleManager.CreateAsync(new IdentityRole(item));
                }
            }
        }
    }
}
