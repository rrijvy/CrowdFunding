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
using Microsoft.AspNetCore.Authorization;
using CrowdFunding.Authorization;

namespace CrowdFunding.Controllers
{
    public class InvestmentTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public InvestmentTypesController(ApplicationDbContext context,
                                        UserManager<ApplicationUser> userManager,
                                        IAuthorizationService authorizationService)
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        // GET: InvestmentTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.investmentTypes.ToListAsync());
        }

        // GET: InvestmentTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investmentType = await _context.investmentTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (investmentType == null)
            {
                return NotFound();
            }

            return View(investmentType);
        }

        // GET: InvestmentTypes/Create
        public IActionResult Create()
        {
            var project = _context.Projects.Include(x => x.Company).ThenInclude(x => x.Entrepreneur);
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewData["ProjectId"] = new SelectList(project.Where(x => x.Company.Entrepreneur.Id == userId), "Id", "Name"); 
            return View();
        }

        // POST: InvestmentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,ShortDescription,ProjectId")] InvestmentType investmentType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(investmentType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(investmentType);
        }

        // GET: InvestmentTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investmentType = await _context.investmentTypes.FindAsync(id);
            if (investmentType == null)
            {
                return NotFound();
            }
            return View(investmentType);
        }

        // POST: InvestmentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type")] InvestmentType investmentType)
        {
            if (id != investmentType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(investmentType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvestmentTypeExists(investmentType.Id))
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
            return View(investmentType);
        }

        // GET: InvestmentTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investmentType = await _context.investmentTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (investmentType == null)
            {
                return NotFound();
            }

            return View(investmentType);
        }

        // POST: InvestmentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var investmentType = await _context.investmentTypes.FindAsync(id);
            _context.investmentTypes.Remove(investmentType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvestmentTypeExists(int id)
        {
            return _context.investmentTypes.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> CreateReward(int? id)
        {
            var project = _context.Projects.Where(x => x.Id == id).Include(x => x.Company).ThenInclude(x=>x.Entrepreneur);

            var checkProjectUserIdModel = new CheckProjectUserIdModel();

            foreach (var item in project)
            {
                checkProjectUserIdModel.EntreprenuerId = item.Company.Entrepreneur.Id;
            }

            var authResult = await _authorizationService.AuthorizeAsync(User, checkProjectUserIdModel, "EditProjectPolicy");

            if (authResult.Succeeded)
            {
                ViewData["ProjectId"] = id;
                return View();
            }
                       
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> CreateReward([Bind("Id,Type,ShortDescription,ProjectId")] InvestmentType model)
        {
            var type = new InvestmentType
            {
                Type = model.Type,
                ShortDescription = model.ShortDescription,
                ProjectId = model.ProjectId
            };
            await _context.investmentTypes.AddAsync(type);
            await _context.SaveChangesAsync();
            return RedirectToAction("ProjectDashboard", "Projects", new { id = model.ProjectId });
        }
    }
}
