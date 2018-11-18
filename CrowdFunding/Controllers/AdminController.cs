﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdFunding.Data;
using CrowdFunding.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrowdFunding.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PendingFunds()
        {
            var pendingFunds = _context.Investments
                                        .Where(x => x.IsApproved == false)
                                        .Include(x => x.Project)
                                        .Include(x => x.InvestmentType)
                                        .Include(x => x.Investor).ToList();
            return View(pendingFunds);
        }

        [HttpPost]
        public async Task<IActionResult> PendingFunds([Bind("Id,InvestmentRegNo,Amount,ProjectId,InvestorId,IsApproved")] Investment model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var fund = new Funded
            {
                InvestmentId = model.Id,
                InvestorId = model.InvestorId,
                ProjectId = model.ProjectId,
                Amount = model.Amount
            };
            var investment = await _context.Investments.FindAsync(model.Id);
            investment.IsApproved = model.IsApproved;
            await _context.SaveChangesAsync();
            await _context.Fundeds.AddAsync(fund);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PendingFunds));
        }
    }
}