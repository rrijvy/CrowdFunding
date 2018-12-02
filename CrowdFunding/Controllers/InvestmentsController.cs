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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;

namespace CrowdFunding.Controllers
{
    [Authorize(Roles ="Investor")]
    public class InvestmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomizedId _customizedId;
        private readonly IGetProjectOwner _getProjectOwner;
        private readonly IEmailSender _emailSender;

        public InvestmentsController(ApplicationDbContext context,
                                    UserManager<ApplicationUser> userManager,
                                    ICustomizedId customizedId,
                                    IGetProjectOwner getProjectOwner,
                                    IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _customizedId = customizedId;
            _getProjectOwner = getProjectOwner;
            _emailSender = emailSender;
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
            var investmentTypes = await _context.investmentTypes.Where(x => x.ProjectId == id).ToListAsync();

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
        public async Task<IActionResult> Create(InvestmentViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var investment = new Investment
            {
                Amount = model.Investment.Amount,
                InvestmentTypeId = model.Investment.InvestmentTypeId,
                ProjectId = model.Project.Id,
                InvestorId = user.Id
            };
            string regNo = _customizedId.InvestmentRegNo(model, user.Id);
            investment.InvestmentRegNo = regNo;

            _context.Investments.Add(investment);
            await _context.SaveChangesAsync();

            var owner = _getProjectOwner.GetOwner(investment.ProjectId);
            var callbackUrl = Url.Page(
                            "/Projects/Details",
                            pageHandler: null,
                            values: new { id = model.Project.Id },
                            protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(owner.Email, model.Project.Name,
                $"{user.Email} pledge {model.Investment.Amount} taka to {model.Project.Name}. Please contact with admin for more details. To see your project <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>");

            return RedirectToAction(nameof(ConfirmPayment));
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


        public IActionResult ConfirmPayment()
        {
            ViewData["BKashNumber"] = "8801717745808";
            return View();
        }

        private bool InvestmentExists(int id)
        {
            return _context.Investments.Any(e => e.Id == id);
        }
    }
}
