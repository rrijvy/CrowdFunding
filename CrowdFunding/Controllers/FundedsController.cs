using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrowdFunding.Data;
using CrowdFunding.Models;
using CrowdFunding.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CrowdFunding.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FundedsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FundedsController(ApplicationDbContext context,
                                UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Fundeds
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Fundeds.Include(f => f.Investment).Include(f => f.Investor).Include(f => f.Project);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Fundeds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funded = await _context.Fundeds
                .Include(f => f.Investment)
                .Include(f => f.Investor)
                .Include(f => f.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funded == null)
            {
                return NotFound();
            }

            return View(funded);
        }

        // GET: Fundeds/Create
        public IActionResult Create(InvestmentViewModel model)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var funded = new Funded
            {
                ProjectId = model.Project.Id,
                InvestorId = userId,
                Amount = model.Investment.Amount
            };

            

            return View();
        }

        // POST: Fundeds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InvestmentId,ProjectId,InvestorId,Amount,RaisedAmount,IsLive")] Funded funded)
        {
            if (ModelState.IsValid)
            {
                _context.Add(funded);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InvestmentId"] = new SelectList(_context.Investments, "Id", "Id", funded.InvestmentId);
            ViewData["InvestorId"] = new SelectList(_context.Investors, "Id", "Id", funded.InvestorId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", funded.ProjectId);
            return View(funded);
        }

        // GET: Fundeds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funded = await _context.Fundeds.FindAsync(id);
            if (funded == null)
            {
                return NotFound();
            }
            ViewData["InvestmentId"] = new SelectList(_context.Investments, "Id", "Id", funded.InvestmentId);
            ViewData["InvestorId"] = new SelectList(_context.Investors, "Id", "Id", funded.InvestorId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", funded.ProjectId);
            return View(funded);
        }

        // POST: Fundeds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InvestmentId,ProjectId,InvestorId,Amount,RaisedAmount,IsLive")] Funded funded)
        {
            if (id != funded.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(funded);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FundedExists(funded.Id))
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
            ViewData["InvestmentId"] = new SelectList(_context.Investments, "Id", "Id", funded.InvestmentId);
            ViewData["InvestorId"] = new SelectList(_context.Investors, "Id", "Id", funded.InvestorId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", funded.ProjectId);
            return View(funded);
        }

        // GET: Fundeds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funded = await _context.Fundeds
                .Include(f => f.Investment)
                .Include(f => f.Investor)
                .Include(f => f.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funded == null)
            {
                return NotFound();
            }

            return View(funded);
        }

        // POST: Fundeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var funded = await _context.Fundeds.FindAsync(id);
            _context.Fundeds.Remove(funded);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FundedExists(int id)
        {
            return _context.Fundeds.Any(e => e.Id == id);
        }
    }
}
