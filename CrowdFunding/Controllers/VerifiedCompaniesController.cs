using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrowdFunding.Data;
using CrowdFunding.Models;
using Microsoft.AspNetCore.Authorization;

namespace CrowdFunding.Controllers
{
    [Authorize(Roles ="Admin")]
    public class VerifiedCompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VerifiedCompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VerifiedCompanies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.VerifiedCompanies.Include(v => v.Company);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: VerifiedCompanies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var verifiedCompany = await _context.VerifiedCompanies
                .Include(v => v.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (verifiedCompany == null)
            {
                return NotFound();
            }

            return View(verifiedCompany);
        }

        // GET: VerifiedCompanies/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Email");
            return View();
        }

        // POST: VerifiedCompanies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompanyId")] VerifiedCompany verifiedCompany)
        {
            if (ModelState.IsValid)
            {
                _context.Add(verifiedCompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Email", verifiedCompany.CompanyId);
            return View(verifiedCompany);
        }

        // GET: VerifiedCompanies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var verifiedCompany = await _context.VerifiedCompanies.FindAsync(id);
            if (verifiedCompany == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Email", verifiedCompany.CompanyId);
            return View(verifiedCompany);
        }

        // POST: VerifiedCompanies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyId")] VerifiedCompany verifiedCompany)
        {
            if (id != verifiedCompany.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(verifiedCompany);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VerifiedCompanyExists(verifiedCompany.Id))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Email", verifiedCompany.CompanyId);
            return View(verifiedCompany);
        }

        // GET: VerifiedCompanies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var verifiedCompany = await _context.VerifiedCompanies
                .Include(v => v.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (verifiedCompany == null)
            {
                return NotFound();
            }

            return View(verifiedCompany);
        }

        // POST: VerifiedCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var verifiedCompany = await _context.VerifiedCompanies.FindAsync(id);
            _context.VerifiedCompanies.Remove(verifiedCompany);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VerifiedCompanyExists(int id)
        {
            return _context.VerifiedCompanies.Any(e => e.Id == id);
        }
    }
}
