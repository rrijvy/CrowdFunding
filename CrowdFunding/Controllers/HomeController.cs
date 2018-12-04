﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CrowdFunding.Models;
using CrowdFunding.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http.Headers;
using CrowdFunding.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CrowdFunding.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace CrowdFunding.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IGetFundedAmount _fundedAmount;
        private readonly IContactEmailSender _emailSender;

        public HomeController(ApplicationDbContext context,
            IHostingEnvironment environment,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IGetFundedAmount fundedAmount,
            IContactEmailSender emailSender)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _signInManager = signInManager;
            _fundedAmount = fundedAmount;
            _emailSender = emailSender;
        }


        public IActionResult Index(int? categoryId)
        {
            List<ProjectCategory> projectCategories = _context.ProjectCategory.ToList();
            List<Project> fileProjects = _context.Projects.Where(x => x.ProjectCategoryId == 1).ToList();
            IQueryable<Project> latestProject = _context.Projects.Include(x => x.Company).ThenInclude(x => x.Entrepreneur).OrderByDescending(x => x.Id).Take(8);
            List<Project> latestProjects = new List<Project>();
            foreach (var item in latestProject)
            {
                latestProjects.Add(item);
            }
            var inputViewModel = new HomeIndexVIewModel
            {
                ProjectCategories = projectCategories,
                FileProjects = fileProjects,
                LatestProject = latestProjects
            };


            if (!(categoryId == null))
            {
                inputViewModel.Projects = _context.Projects.Where(x => x.ProjectCategoryId == categoryId).ToList();
                inputViewModel.LastProject = _context.Projects.Where(x => x.ProjectCategoryId == categoryId).LastOrDefault();
            }
            else
            {
                inputViewModel.Projects = null;
                inputViewModel.LastProject = _context.Projects.Where(x => x.ProjectCategoryId == 1).LastOrDefault();
            }

            return View(inputViewModel);
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Contact(string email, string subject, string message)
        {
            var htmlMessage = message + " reply to " + email;
            await _emailSender.SendContactEmailAsync(email, subject, htmlMessage);
            return RedirectToAction(nameof(Thanks));
        }

        public IActionResult Thanks()
        {
            return View();
        }

        public IActionResult Works()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
               
        [HttpPost]
        public IActionResult UploadFilesAjax()
        {
            long size = 0;
            var files = Request.Form.Files;
            string location = "";
            foreach (var file in files)
            {
                location = Path.Combine(_environment.WebRootPath, Path.GetFileName(file.FileName));
                var filename = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName
                                .Trim('"');
                filename = _environment.WebRootPath + $@"\{filename}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }

            }
            return Json(location);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RemindProject(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var favProjects = _context.Favourites.Where(x => x.UserId == user.Id).ToList();
            bool isExist = favProjects.Any(x => x.ProjectId == id);
            if (!isExist == true)
            {
                var favourite = new Favourite
                {
                    UserId = user.Id,
                    ProjectId = id
                };

                _context.Favourites.Add(favourite);
                await _context.SaveChangesAsync();
                return Json(new { value = "Added" });
            }
            return Json(new { value = "Already exist" });
        }

        [HttpPost]
        public IActionResult Search(string keyword, int page)
         {
            int numberOfContent = 12;

            var projectItems = _context.Projects
               .Where(x => x.Name.Contains(keyword) || x.Company.Name.Contains(keyword) || x.ProjectShortDescription.Contains(keyword) || x.ProjectCategory.Type.Contains(keyword))
               .Include(x=> x.ProjectCategory)
               .Include(x => x.Company)
               .ThenInclude(x => x.Entrepreneur)
               .ThenInclude(x => x.Country)
               .OrderByDescending(x=>x.Id);

            int numberOfPage = projectItems.Count() / numberOfContent;

            List<ProjectViewModel> projectList = new List<ProjectViewModel>();                          

            var projects = projectItems
                .Skip((page - 1) * numberOfContent)
                .Take(numberOfContent).ToList();

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

            ViewData["NumberOfPage"] = numberOfPage;
            ViewData["Keyword"] = keyword;
            ViewData["Country"] = new SelectList(_context.Countries, "Id", "Name");
            ViewData["SortedBy"] = new SelectList(_context.Countries, "Id", "Name");
            return View(projectList);
        }

        [HttpPost]
        public IActionResult SearchByCountry(string keyword, int countryId, int page)
        {
            int numberOfContent = 12;

            var projectItems = _context.Projects
               .Where(x => x.Name.Contains(keyword) || x.Company.Name.Contains(keyword) || x.ProjectShortDescription.Contains(keyword) || x.ProjectCategory.Type.Contains(keyword))
               .Include(x => x.ProjectCategory)
               .Include(x => x.Company)
               .ThenInclude(x => x.Entrepreneur)
               .ThenInclude(x => x.Country)
               .OrderByDescending(x => x.Id);

            int numberOfPage = projectItems.Count() / numberOfContent;

            List<ProjectViewModel> projectList = new List<ProjectViewModel>();

            var projects = projectItems
                .Skip((page - 1) * numberOfContent)
                .Take(numberOfContent).ToList();

            foreach (var item in projects)
            {
                if (item.Company.Entrepreneur.Country.Id == countryId)
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
            }

            ViewData["NumberOfPage"] = numberOfPage;
            ViewData["Keyword"] = keyword;
            ViewData["Country"] = new SelectList(_context.Countries, "Id", "Name");
            return View(projectList);
        }

        [HttpGet]
        public async Task<IActionResult> UserDetails(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var country = _context.Countries.Find(user.CountryId);
            var companies = _context.Companies.Where(x => x.EntrepreneurId == user.Id);
            var backed = _context.Investments.Where(x => x.InvestorId == user.Id);
            var content = new UserViewModel
            {
                Name = user.FName + " " + user.LName,
                Email = user.Email,
                IsVerified = user.EmailConfirmed,
                Companies = companies.Count(),
                Country = country.Name,
                Backed = backed.Count(),
                Avater = user.Image                
            };
            return Json(content);
        }

        [HttpPost]
        public async Task<IActionResult> UploadUserAvater(IFormFile userAvater)
        {
            if (!(userAvater == null))
            {
                var user = await _userManager.GetUserAsync(User);
                var fileName = ContentDispositionHeaderValue.Parse(userAvater.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                var fileExtension = Path.GetExtension(userAvater.FileName).ToLower();

                using (var stream = new FileStream(GetPath(fileName, user.Email), FileMode.Create))
                {
                    await userAvater.CopyToAsync(stream);
                }

                return Ok();
            }
            return BadRequest();

        }

        private string GetPath(string fileName, string userEmail)
        {
            string path = _environment.WebRootPath + "\\images\\" + userEmail + "\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path + fileName;
        }
    }
}
