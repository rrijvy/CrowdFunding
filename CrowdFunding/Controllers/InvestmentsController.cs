using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrowdFunding.Data;
using CrowdFunding.Models;
using CrowdFunding.ViewModels;
using Microsoft.AspNetCore.Identity;
using CrowdFunding.Services;

namespace CrowdFunding.Controllers
{
    public class InvestmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomizedId _customizedId;

        public InvestmentsController(ApplicationDbContext context,
                                    UserManager<ApplicationUser> userManager,
                                    ICustomizedId customizedId)
        {
            _context = context;
            _userManager = userManager;
            _customizedId = customizedId;
        }

        // GET: Investments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Investments.Include(i => i.InvestmentType).Include(i => i.Investor).Include(i => i.Project);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Investments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investment = await _context.Investments
                .Include(i => i.InvestmentType)
                .Include(i => i.Investor)
                .Include(i => i.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (investment == null)
            {
                return NotFound();
            }

            return View(investment);
        }

        // GET: Investments/Create
        public async Task<IActionResult> Create(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            
            var investmentTypes = await _context.investmentTypes.ToListAsync();
            var investmentViewModel = new InvestmentViewModel
            {
                Project = project,
                InvestmentTypes = investmentTypes
                
            };

            return View(investmentViewModel);
        }

        // POST: Investments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Investment model)
        {
            var userId = _userManager.GetUserId(HttpContext.User);            
            var investment = new Investment
            {
                Amount = model.Amount,
                InvestmentTypeId = model.InvestmentTypeId,
                ProjectId = model.ProjectId,
                InvestorId = userId
            };
            var regNo = _customizedId.InvestmentRegNo(model, userId);



            return View(investment);
        }

        // GET: Investments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investment = await _context.Investments.FindAsync(id);
            if (investment == null)
            {
                return NotFound();
            }
            ViewData["InvestmentTypeId"] = new SelectList(_context.investmentTypes, "Id", "Id", investment.InvestmentTypeId);
            ViewData["InvestorId"] = new SelectList(_context.Investors, "Id", "Id", investment.InvestorId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", investment.ProjectId);
            return View(investment);
        }

        // POST: Investments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InvestmentRegNo,Amount,ProjectId,InvestmentTypeId,InvestorId")] Investment investment)
        {
            if (id != investment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(investment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvestmentExists(investment.Id))
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
            ViewData["InvestmentTypeId"] = new SelectList(_context.investmentTypes, "Id", "Id", investment.InvestmentTypeId);
            ViewData["InvestorId"] = new SelectList(_context.Investors, "Id", "Id", investment.InvestorId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", investment.ProjectId);
            return View(investment);
        }

        // GET: Investments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investment = await _context.Investments
                .Include(i => i.InvestmentType)
                .Include(i => i.Investor)
                .Include(i => i.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (investment == null)
            {
                return NotFound();
            }

            return View(investment);
        }

        // POST: Investments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var investment = await _context.Investments.FindAsync(id);
            _context.Investments.Remove(investment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvestmentExists(int id)
        {
            return _context.Investments.Any(e => e.Id == id);
        }
    }
}
