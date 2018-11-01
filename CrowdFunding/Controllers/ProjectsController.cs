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
using CrowdFunding.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace CrowdFunding.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGetFundedAmount _fundedAmount;

        public ProjectsController(ApplicationDbContext context,
                                    IGetFundedAmount fundedAmount)
        {
            _context = context;
            _fundedAmount = fundedAmount;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Projects.Include(p => p.Company).Include(p => p.ProjectCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Projects/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = _context.Projects
                            .Include(x => x.Company)
                            .ThenInclude(x => x.Entrepreneur)
                            .ThenInclude(x => x.Country)
                            .FirstOrDefault(x => x.Id == id);
            var projectViewModel = new ProjectViewModel
            {
                Id = project.Id,
                Image = project.Image1,
                Name = project.Name,
                ShortDescription = project.ProjectShortDescription,
                LongDescription = project.DetailDescription,
                EntreprenuerName = project.Company.Entrepreneur.FName + " " + project.Company.Entrepreneur.LName,
                PledgedAmount = project.NeededFund,
                DaysLeft = Math.Floor((project.EndingDate - DateTime.Now).TotalDays),
                CompanyName = project.Company.Name,
                CountryName = project.Company.Entrepreneur.Country.Name,
                Funded = _fundedAmount.FundedAmount(project.Id)
            };

            if (projectViewModel == null)
            {
                return NotFound();
            }

            return View(projectViewModel);
        }


        public IActionResult CreateProject()
        {
            return View();
        }

        public IActionResult SelectCategory()
        {
            ViewData["ProjectCategory"] = new SelectList(_context.ProjectCategory, "Id", "Type");
            return View();
        }

        [HttpPost]
        public IActionResult ShortDescription(ProjectCategory projectCategory)
        {
            var project = new Project
            {
                ProjectCategoryId = projectCategory.Id
            };
            return View(project);
        }

        [HttpPost]
        public IActionResult Create(Project model)
        {
            var project = new Project
            {
                ProjectShortDescription = model.ProjectShortDescription,
                ProjectCategoryId = model.ProjectCategoryId
            };
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            return View(project);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost([Bind("Id,Name,ProjectShortDescription,ProjectCategoryId,ProjectTitle,NeededFund,StartingDate,EndingDate,CompanyId")] Project model)
        {
            var project = new Project
            {
                Name = model.Name,
                ProjectShortDescription = model.ProjectShortDescription,
                ProjectCategoryId = model.ProjectCategoryId,
                ProjectTitle = model.ProjectTitle,
                NeededFund = model.NeededFund,
                StartingDate = model.StartingDate,
                EndingDate = model.EndingDate,
                CompanyId = model.CompanyId
            };
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return RedirectToAction("ProjectDashboard", new { id = project.Id });
        }

        public async Task<IActionResult> ProjectDashboard(int id)
        {
            Project project = await _context.Projects.FindAsync(id);
            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Policy = "EditProjectPolicy")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Email", project.CompanyId);
            ViewData["ProjectCategoryId"] = new SelectList(_context.Set<ProjectCategory>(), "Id", "Id", project.ProjectCategoryId);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ProjectShortDescription,DetailDescription,ProjectTitle,IsRunning,IsCompleted,NeededFund,StartingDate,EndingDate,Image1,Image2,Image3,CompanyId,ProjectCategoryId")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Email", project.CompanyId);
            ViewData["ProjectCategoryId"] = new SelectList(_context.Set<ProjectCategory>(), "Id", "Id", project.ProjectCategoryId);
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Company)
                .Include(p => p.ProjectCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
        

        public IActionResult ShowByCategory(int id)
        {
            var projects = _context.Projects.Where(x => x.ProjectCategoryId == id).ToList();
            return Json(projects);
        }

        public IActionResult ShowProjectByCategory(int id)
        {
            List<ProjectViewModel> projectList = new List<ProjectViewModel>();
            var projects = _context.Projects.Where(x => x.ProjectCategoryId == id).Include(x=>x.Company).ThenInclude(x=>x.Entrepreneur).ThenInclude(x=>x.Country);
            
            foreach (var item in projects)
            {
                var projectViewModel = new ProjectViewModel
                {
                    Id = item.Id,
                    Image = item.Image1,
                    Name = item.Name,
                    ShortDescription = item.ProjectShortDescription,
                    EntreprenuerName = item.Company.Entrepreneur.FName + " " + item.Company.Entrepreneur.LName,
                    PledgedAmount = item.NeededFund,
                    DaysLeft = Math.Floor((item.EndingDate - DateTime.Now).TotalDays),
                    CompanyName = item.Company.Name,
                    CountryName = item.Company.Entrepreneur.Country.Name,
                    Funded = _fundedAmount.FundedAmount(item.Id)
                };
                projectList.Add(projectViewModel);


            }
            
            
            return View(projectList);
        }

        
    }
}
