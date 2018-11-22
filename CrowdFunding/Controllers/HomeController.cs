using System;
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

namespace CrowdFunding.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(ApplicationDbContext context,
            IHostingEnvironment environment,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index(int? categoryId)
        {
            var projectCategories = _context.ProjectCategory.ToList();
            var projects = _context.Projects.Where(x => x.ProjectCategoryId == categoryId).ToList();
            var fileProjects = _context.Projects.Where(x => x.ProjectCategoryId == 1).ToList();

            var inputViewModel = new HomeIndexVIewModel
            {
                ProjectCategories = projectCategories,
                Projects = projects,
                FileProjects = fileProjects
            };

            if (categoryId == null)
            {
                inputViewModel.LastProject = _context.Projects.Where(x => x.ProjectCategoryId == 1).LastOrDefault();
            }
            else
            {
                inputViewModel.LastProject = _context.Projects.Where(x => x.ProjectCategoryId == categoryId).LastOrDefault();
            }

            return View(inputViewModel);
        }

        public IActionResult About()
        {
            return Json(_context.Entrepreneurs.ToList());
        }

        public IActionResult Contact()
        {
            return Json(_context.Investors.ToList());
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

            //var fileName = Path.Combine(he.WebRootPath, Path.GetFileName(files.FileName));

            //string message = $"{files.Count} file(s) / { size} bytes uploaded successfully!";
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
