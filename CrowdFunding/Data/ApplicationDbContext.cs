using CrowdFunding.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CrowdFunding.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyType> CompanyTypes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Entrepreneur> Entrepreneurs { get; set; }
        public DbSet<Funded> Fundeds { get; set; }
        public DbSet<Investment> Investments { get; set; }
        public DbSet<InvestmentType> investmentTypes { get; set; }
        public DbSet<Investor> Investors { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<VerifiedCompany> VerifiedCompanies { get; set; }
    }
}
