using CrowdFunding.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace CrowdFunding.Services
{
    public static class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context, IServiceProvider service)
        {
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();

            var admin = new ApplicationUser
            {
                FName = "Rijvy",
                LName = "Admin",
                Email = "admin@cf.com",
                UserName = "admin@cf.com",
                CountryId = 1,
                NID = "46455645878",
                PresentAddress = "Mirpur",
                ParmanantAddress = "Narail"
            };
            var moderator = new ApplicationUser
            {
                FName = "Rijvy",
                LName = "Moderator",
                Email = "moderator@cf.com",
                UserName = "moderator@cf.com",
                CountryId = 1,
                NID = "66455645878",
                PresentAddress = "Mirpur",
                ParmanantAddress = "Narail"
            };

            string[] roleNames = { "Admin", "Entreprenuer", "Investor", "Moderator" };

            IdentityResult roles;
            foreach (var item in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(item);
                if (!roleExist)
                {
                    roles = await roleManager.CreateAsync(new IdentityRole(item));
                }
            }

            if (await userManager.FindByNameAsync(admin.Email) == null)
            {
                await userManager.CreateAsync(admin, "a1234Z-");
                await userManager.AddToRoleAsync(admin, "Admin");
                
            }

            if (await userManager.FindByNameAsync(moderator.Email) == null)
            {
                await userManager.CreateAsync(moderator, "a1234Z-");
                await userManager.AddToRoleAsync(moderator, "Moderator");
            }
        }
    }
}
