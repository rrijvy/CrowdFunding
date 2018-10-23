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

namespace CrowdFunding.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;



        public HomeController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public IActionResult Index()
        {
            var projectCategory = _context.ProjectCategory.ToList();
            ViewData["Projects"] = _context.Projects.ToList();
            return View(projectCategory);
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
    }
}
