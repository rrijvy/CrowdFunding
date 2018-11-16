using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrowdFunding.Data;
using CrowdFunding.Models;
using Microsoft.AspNetCore.Identity;


namespace CrowdFunding.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public CompaniesController(ApplicationDbContext context,
                                    UserManager<ApplicationUser> userManager,
                                    SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Companies.Include(c => c.CompanyType).Include(c => c.Entrepreneur);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .Include(c => c.CompanyType)
                .Include(c => c.Entrepreneur)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }
             

        public IActionResult SelectType()
        {
            ViewData["CompanyTypes"] = new SelectList(_context.CompanyTypes, "Id", "TypeName");
            return View();
        }


        [HttpPost]
        public IActionResult Create(CompanyType model)
        {
            var company = new Company
            {
                CompanyTypeId = model.Id
            };
            return View(company);
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(Company model)
        {
            if (ModelState.IsValid)
            {
                var company = new Company
                {
                    Name = model.Name,
                    CompanyTypeId = model.CompanyTypeId,
                    Email = model.Email,
                    Liesence = model.Liesence,
                    PhoneNo = model.PhoneNo,
                    Address = model.Address,
                    WebsiteUrl = model.WebsiteUrl
                };

                var userId = _userManager.GetUserId(HttpContext.User);

                company.EntrepreneurId = userId;

                if (string.IsNullOrEmpty(model.RegNo))
                    company.RegNo = null;
                else
                    company.RegNo = model.RegNo;

                await _context.AddAsync(company);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            ViewData["CompanyTypeId"] = new SelectList(_context.CompanyTypes, "Id", "TypeName", company.CompanyTypeId);
            ViewData["EntrepreneurId"] = new SelectList(_context.Entrepreneurs, "Id", "Id", company.EntrepreneurId);
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,RegNo,TypeName,EntrepreneurId,Email,Liesence,PhoneNo,Address,WebsiteUrl,Video")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyTypeId"] = new SelectList(_context.CompanyTypes, "Id", "TypeName", company.CompanyTypeId);
            ViewData["EntrepreneurId"] = new SelectList(_context.Entrepreneurs, "Id", "Id", company.EntrepreneurId);
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .Include(c => c.CompanyType)
                .Include(c => c.Entrepreneur)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
