using System.Linq;
using System.Threading.Tasks;
using CrowdFunding.Data;
using CrowdFunding.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrowdFunding.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PendingFunds()
        {
            var pendingFunds = _context.Investments
                                        .Where(x => x.IsApproved == false)
                                        .Include(x => x.Project)
                                        .Include(x => x.InvestmentType)
                                        .Include(x => x.Investor).ToList();
            return View(pendingFunds);
        }

        [HttpPost]
        public async Task<IActionResult> PendingFunds([Bind("Id,InvestmentRegNo,Amount,ProjectId,InvestorId,IsApproved")] Investment model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var fund = new Funded
            {
                InvestmentId = model.Id,
                InvestorId = model.InvestorId,
                ProjectId = model.ProjectId,
                Amount = model.Amount
            };
            var investment = await _context.Investments.FindAsync(model.Id);
            investment.IsApproved = model.IsApproved;
            await _context.SaveChangesAsync();
            await _context.Fundeds.AddAsync(fund);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PendingFunds));
        }

        [HttpGet]
        public IActionResult PendingCompanies()
        {
            var companies = _context.Companies
                                            .Where(x => x.IsVerified == false)
                                            .Include(x => x.Projects)
                                            .Include(x => x.Entrepreneur)
                                            .ThenInclude(x => x.Country).ToList();
            return View(companies);
        }

        [HttpPost]
        public IActionResult PendingCompanies(int id)
        {
            if (id == 0)
            {
                var company = _context.Companies.Find(id);
                company.IsVerified = true;
                _context.Update(company);
                _context.SaveChanges();
                return RedirectToAction(nameof(PendingCompanies));
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult PendingProjects()
        {
            var projects = _context.Projects
                                            .Where(x => x.IsVerified == false)
                                            .Include(x => x.Company)
                                            .ThenInclude(x => x.Entrepreneur)
                                            .ThenInclude(x => x.Country).ToList();
            return View(projects);
        }

        [HttpPost]
        public IActionResult PendingProjects(int id)
        {
            if (id == 0)
            {
                var projects = _context.Projects.Find(id);
                projects.IsVerified = true;
                _context.Update(projects);
                _context.SaveChanges();
                return RedirectToAction(nameof(PendingProjects));
            }
            return NotFound();

        }
    }
}