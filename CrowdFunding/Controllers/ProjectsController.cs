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
using CrowdFunding.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CrowdFunding.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGetFundedAmount _fundedAmount;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _environment;

        public ProjectsController(ApplicationDbContext context,
                                    IHostingEnvironment environment,
                                    IGetFundedAmount fundedAmount,
                                    IAuthorizationService authorizationService,
                                    UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _fundedAmount = fundedAmount;
            _authorizationService = authorizationService;
            _userManager = userManager;
            _environment = environment;
        }


        // GET: Projects
        [AllowAnonymous]
        public IActionResult Index()
        {
            List<ProjectViewModel> projectList = new List<ProjectViewModel>();
            var projects = _context.Projects.Include(x => x.Company).ThenInclude(x => x.Entrepreneur).ThenInclude(x => x.Country);

            foreach (var item in projects)
            {
                var projectViewModel = new ProjectViewModel
                {
                    Id = item.Id,
                    Image = item.Image1,
                    Name = item.Name,
                    ProjectTitle = item.ProjectTitle,
                    ShortDescription = item.ProjectShortDescription,
                    EntreprenuerName = item.Company.Entrepreneur.FName + " " + item.Company.Entrepreneur.LName,
                    PledgedAmount = item.NeededFund,
                    DaysLeft = Math.Floor((item.EndingDate - DateTime.Now).TotalDays),
                    CompanyName = item.Company.Name,
                    CountryName = item.Company.Entrepreneur.Country.Name,
                    Funded = _fundedAmount.FundedAmount(item.Id),
                    Image2 = item.Image2,
                    Image3 = item.Image3,
                    EntreprenuerId = item.Company.Entrepreneur.Id
                };
                projectList.Add(projectViewModel);
            }


            return View(projectList.OrderByDescending(x => x.Id));
        }

        // GET: Projects/Details/5
        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = _context.Projects.Find(id);
            model.Viewed++;
            try
            {
                _context.Update(model);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(model.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var projects = _context.Projects
                            .Where(x => x.Id == id)
                            .Include(x => x.Company)
                            .ThenInclude(x => x.Entrepreneur)
                            .ThenInclude(x => x.Country);
            var projectViewModel = new ProjectViewModel();
            foreach (var project in projects)
            {
                projectViewModel.Id = project.Id;
                projectViewModel.Image = project.Image1;
                projectViewModel.Name = project.Name;
                projectViewModel.ProjectTitle = project.ProjectTitle;
                projectViewModel.ShortDescription = project.ProjectShortDescription;
                projectViewModel.LongDescription = project.DetailDescription;
                projectViewModel.EntreprenuerName = project.Company.Entrepreneur.FName + " " + project.Company.Entrepreneur.LName;
                projectViewModel.PledgedAmount = project.NeededFund;
                projectViewModel.DaysLeft = Math.Floor((project.EndingDate - DateTime.Now).TotalDays);
                projectViewModel.CompanyName = project.Company.Name;
                projectViewModel.CountryName = project.Company.Entrepreneur.Country.Name;
                projectViewModel.Funded = _fundedAmount.FundedAmount(project.Id);
                projectViewModel.TotalBacker = _fundedAmount.Backers(project.Id);
                projectViewModel.Image2 = project.Image2;
                projectViewModel.Image3 = project.Image3;
                projectViewModel.VideoUrl = project.VideoUrl;
                projectViewModel.Viewed = project.Viewed;
            }
            if (projectViewModel == null)
            {
                return NotFound();
            }
            return View(projectViewModel);
        }

        [Authorize(Roles = "Entreprenuer")]
        public async Task<IActionResult> CreateProject()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var companies = _context.Companies.Where(x => x.EntrepreneurId == userId);

            var company = new Company();

            foreach (var item in companies)
            {
                company = item;
            }

            if (company.Id == 0)
                return RedirectToAction("SelectType", "Companies");

            return View();
        }

        [Authorize(Roles = "Entreprenuer")]
        public IActionResult SelectCategory()
        {
            ViewData["ProjectCategory"] = new SelectList(_context.ProjectCategory, "Id", "Type");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Entreprenuer")]
        public IActionResult ShortDescription(ProjectCategory projectCategory)
        {
            var project = new Project
            {
                ProjectCategoryId = projectCategory.Id
            };
            return View(project);
        }


        [HttpPost]
        [Authorize(Roles = "Entreprenuer")]
        public async Task<IActionResult> Create(Project model)
        {
            var project = new Project
            {
                ProjectShortDescription = model.ProjectShortDescription,
                ProjectCategoryId = model.ProjectCategoryId
            };
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewData["CompanyId"] = new SelectList(_context.Companies.Where(x => x.EntrepreneurId == (user.Id).ToString()), "Id", "Name");
            return View(project);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Entreprenuer")]
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


        //Get projectdashboard
        [Authorize(Roles = "Entreprenuer")]
        public async Task<IActionResult> ProjectDashboard(int? id)
        {
            var proj = _context.Projects.Where(x => x.Id == id).Include(x => x.Company);

            var checkProjectUserIdModel = new CheckProjectUserIdModel();
            foreach (var item in proj)
            {
                checkProjectUserIdModel.EntreprenuerId = item.Company.EntrepreneurId;
            }

            var authResult = await _authorizationService.AuthorizeAsync(User, checkProjectUserIdModel, "EditProjectPolicy");

            if (authResult.Succeeded)
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
                ViewData["ProjectCategoryId"] = new SelectList(_context.ProjectCategory, "Id", "Name");

                return View(project);
            }
            return RedirectToAction("Index", "Home");
        }


        //post projectdashboard
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Entreprenuer")]
        public async Task<IActionResult> ProjectDashboard(int id, List<IFormFile> files, [Bind("Id,Name,ProjectShortDescription,DetailDescription,ProjectTitle,IsRunning,IsCompleted,NeededFund,StartingDate,EndingDate,CompanyId,ProjectCategoryId,VideoUrl")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            string fileNames = string.Empty;

            foreach (var item in files)
            {
                string fileName = ContentDispositionHeaderValue.Parse(item.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                string sFileExtension = Path.GetExtension(item.FileName).ToLower();
                if ((sFileExtension == ".jpg") || (sFileExtension == ".jpge") || (sFileExtension == ".png") || (sFileExtension == ".bmp"))
                {
                    using (var stream = new FileStream(GetPath(fileName, project.ProjectTitle), FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                        fileNames += fileName + ",";
                    }
                }
            }

            string[] nameOfFile = fileNames.Split(",");
            int nameLength = nameOfFile.Length;
            if (nameLength - 1 == 1)
            {
                if (!string.IsNullOrEmpty(nameOfFile[0]))
                {
                    project.Image1 = nameOfFile[0];
                }
            }
            if (nameLength == 2)
            {
                if (!string.IsNullOrEmpty(nameOfFile[0]))
                {
                    project.Image1 = nameOfFile[0];
                }
                if (!string.IsNullOrEmpty(nameOfFile[1]))
                {
                    project.Image2 = nameOfFile[1];
                }
            }

            if (nameLength >= 3)
            {
                if (!string.IsNullOrEmpty(nameOfFile[0]))
                {
                    project.Image1 = nameOfFile[0];
                }
                if (!string.IsNullOrEmpty(nameOfFile[1]))
                {
                    project.Image2 = nameOfFile[1];
                }
                if (!string.IsNullOrEmpty(nameOfFile[2]))
                {
                    project.Image3 = nameOfFile[2];
                }
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
            }

            return View(project);
        }


        // GET: Projects/Edit/5
        [Authorize(Roles = "Entreprenuer")]
        public async Task<IActionResult> Edit(int? id)
        {
            var proj = _context.Projects.Where(x => x.Id == id).Include(x => x.Company);

            var checkProjectUserIdModel = new CheckProjectUserIdModel();
            foreach (var item in proj)
            {
                checkProjectUserIdModel.EntreprenuerId = item.Company.EntrepreneurId;
            }

            var authResult = await _authorizationService.AuthorizeAsync(User, checkProjectUserIdModel, "EditProjectPolicy");

            if (authResult.Succeeded)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var project = _context.Projects.Find(id);

                if (project == null)
                {
                    return NotFound();
                }

                ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Email", project.CompanyId);
                ViewData["ProjectCategoryId"] = new SelectList(_context.Set<ProjectCategory>(), "Id", "Id", project.ProjectCategoryId);
                return View(project);
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Entreprenuer")]
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
        [Authorize(Roles = "Entreprenuer")]
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
        [Authorize(Roles = "Entreprenuer")]
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

        [AllowAnonymous]
        public IActionResult ShowByCategory(int id)
        {
            List<ProjectViewModel> projectList = new List<ProjectViewModel>();
            var projects = _context.Projects.Where(x => x.ProjectCategoryId == id && x.IsVerified == true).OrderByDescending(x => x.Id)
                                            .Include(x => x.Company)
                                            .ThenInclude(x => x.Entrepreneur)
                                            .ThenInclude(x => x.Country);
            foreach (var item in projects)
            {
                var projectViewModel = new ProjectViewModel
                {
                    Id = item.Id,
                    Image = item.Image1,
                    Name = item.Name,
                    ProjectTitle = item.ProjectTitle,
                    ShortDescription = item.ProjectShortDescription,
                    EntreprenuerName = item.Company.Entrepreneur.FName + " " + item.Company.Entrepreneur.LName,
                    PledgedAmount = item.NeededFund,
                    DaysLeft = Math.Floor((item.EndingDate - DateTime.Now).TotalDays),
                    CompanyName = item.Company.Name,
                    CountryName = item.Company.Entrepreneur.Country.Name,
                    Funded = _fundedAmount.FundedAmount(item.Id),
                    Image2 = item.Image2,
                    Image3 = item.Image3
                };
                projectList.Add(projectViewModel);
            }
            return Json(projectList);
        }

        [AllowAnonymous]
        public IActionResult ShowProjectByCategory(int id)
        {
            List<ProjectViewModel> projectList = new List<ProjectViewModel>();
            var projects = _context.Projects.Where(x => x.ProjectCategoryId == id && x.IsVerified == true)
                                            .Include(x => x.Company)
                                            .ThenInclude(x => x.Entrepreneur)
                                            .ThenInclude(x => x.Country);
            foreach (var item in projects)
            {
                var projectViewModel = new ProjectViewModel
                {
                    Id = item.Id,
                    Image = item.Image1,
                    Name = item.Name,
                    ProjectTitle = item.ProjectTitle,
                    ShortDescription = item.ProjectShortDescription,
                    EntreprenuerName = item.Company.Entrepreneur.FName + " " + item.Company.Entrepreneur.LName,
                    PledgedAmount = item.NeededFund,
                    DaysLeft = Math.Floor((item.EndingDate - DateTime.Now).TotalDays),
                    CompanyName = item.Company.Name,
                    CountryName = item.Company.Entrepreneur.Country.Name,
                    Funded = _fundedAmount.FundedAmount(item.Id),
                    Image2 = item.Image2,
                    Image3 = item.Image3,
                    EntreprenuerId = item.Company.EntrepreneurId
                };
                projectList.Add(projectViewModel);
            }
            return View(projectList);
        }

        [Authorize(Roles = "Entreprenuer")]
        public async Task<IActionResult> UserProject()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var projects = _context.Projects.Include(x => x.Company).Where(x => x.Company.EntrepreneurId == user.Id);

            return View(projects);
        }

        [Authorize(Roles = "Investor")]
        public async Task<IActionResult> BakcedProject()
        {
            List<BackedProjectViewModel> BackedProjectList = new List<BackedProjectViewModel>();
            var user = await _userManager.GetUserAsync(User);
            var backs = _context.Investments.Include(x => x.InvestmentType).Include(x => x.Project).Where(x => x.InvestorId == user.Id);
            foreach (var item in backs)
            {
                var BackModel = new BackedProjectViewModel
                {
                    ProjectId = item.ProjectId,
                    ProjectName = item.Project.Name,
                    BackedAmount = item.Amount,
                    ChoosenReward = item.InvestmentType.Type,
                    ProjectTitle = item.Project.ProjectTitle,
                    Image = item.Project.Image1,
                    TotalAmountBacked = _fundedAmount.FundedAmount(item.ProjectId)
                };

                BackedProjectList.Add(BackModel);
            }

            return View(BackedProjectList);
        }

        [Authorize(Roles = "Investor, Entreprenuer")]
        public async Task<IActionResult> LovedProjects()
        {
            var user = await _userManager.GetUserAsync(User);
            List<LovedProjectsViewModel> lovedProjects = new List<LovedProjectsViewModel>();
            var favourites = _context.Favourites.Where(x => x.UserId == user.Id).ToList();
            foreach (var item in favourites)
            {
                var projects = _context.Projects.Find(item.ProjectId);
                var lovedProjectsViewModel = new LovedProjectsViewModel
                {
                    ProjectId = projects.Id,
                    Name = projects.Name,
                    ShortDescription = projects.ProjectShortDescription,
                    StartingDate = projects.StartingDate,
                    EndingDate = projects.EndingDate,
                    Image = projects.Image1,
                    NeededFund = projects.NeededFund,
                    ProjectTitle = projects.ProjectTitle,
                    FavouriteId = item.Id
                };
                lovedProjects.Add(lovedProjectsViewModel);

            }
            return View(lovedProjects);
        }

        [Authorize(Roles = "Investor, Entreprenuer")]
        public IActionResult DeleteFavourite(int id)
        {
            var deleteItem = _context.Favourites.Find(id);
            _context.Favourites.Remove(deleteItem);
            _context.SaveChanges();
            return RedirectToAction(nameof(LovedProjects));
        }


        private string GetPath(string fileName, string projectTitle)
        {
            string title = projectTitle.Replace(" ", string.Empty);
            string path = _environment.WebRootPath + "\\images\\" + title + '\\';
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path + fileName;
        }


    }
}
