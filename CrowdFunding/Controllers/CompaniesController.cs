using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrowdFunding.Data;
using CrowdFunding.Models;

namespace CrowdFunding.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
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

        // GET: Companies/Create
        public IActionResult Create()
        {
            ViewData["CompanyTypeId"] = new SelectList(_context.CompanyTypes, "Id", "TypeName");
            ViewData["EntrepreneurId"] = new SelectList(_context.Entrepreneurs, "Id", "Id");

            var rg = _context.Companies.OrderByDescending(e => e.Id).FirstOrDefault();
            string rgNo = string.Empty;
            string defaultValue = "11";
            int id = 0;
            if (rg == null)
            {
                id = 1;
                rgNo = defaultValue+id.ToString().Trim().PadLeft(5, '0');


            }
            else
            {
                string newId = rg.RegNo.ToString();
                id = int.Parse(newId);
                id += 1;
                rgNo = defaultValue+id.ToString().Trim().PadLeft(5, '0');

            }
            ViewBag.registration = rgNo;


            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,RegNo,TypeName,EntrepreneurId,Email,Liesence,PhoneNo,Address,WebsiteUrl,Video")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyTypeId"] = new SelectList(_context.CompanyTypes, "Id", "TypeName", company.CompanyTypeId);
            ViewData["EntrepreneurId"] = new SelectList(_context.Entrepreneurs, "Id", "Id", company.EntrepreneurId);
            return View(company);
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
