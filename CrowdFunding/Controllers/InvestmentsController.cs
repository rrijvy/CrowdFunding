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
    public class InvestmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvestmentsController(ApplicationDbContext context)
        {
            _context = context;
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
        public IActionResult Create()
        {
            ViewData["InvestmentTypeId"] = new SelectList(_context.investmentTypes, "Id", "Id");
            ViewData["InvestorId"] = new SelectList(_context.Investors, "Id", "Id");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");


            var rg = _context.Investments.OrderByDescending(e => e.Id).FirstOrDefault();
            string rgNo = string.Empty;
            string defaultValue = "33";
            int id = 0;
            if (rg == null)
            {
                id = 1;
                rgNo = defaultValue + id.ToString().Trim().PadLeft(5, '0');


            }
            else
            {
                string newId = rg.InvestmentRegNo.ToString();
                id = int.Parse(newId);
                id += 1;
                rgNo = defaultValue + id.ToString().Trim().PadLeft(5, '0');

            }
            ViewBag.registration = rgNo;


            return View();
        }

        // POST: Investments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InvestmentRegNo,Amount,ProjectId,InvestmentTypeId,InvestorId")] Investment investment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(investment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InvestmentTypeId"] = new SelectList(_context.investmentTypes, "Id", "Id", investment.InvestmentTypeId);
            ViewData["InvestorId"] = new SelectList(_context.Investors, "Id", "Id", investment.InvestorId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", investment.ProjectId);
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
