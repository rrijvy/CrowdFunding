using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdFunding.Data;
using CrowdFunding.Models;
using Microsoft.EntityFrameworkCore;

namespace CrowdFunding.Services
{
    public class GetProjectOwner : IGetProjectOwner
    {
        private ApplicationDbContext _context;

        public GetProjectOwner(ApplicationDbContext context)
        {
            _context = context;
        }
        public Entrepreneur GetOwner(int projectId)
        {
            var project = _context.Projects.Where(x => x.Id == projectId).Include(x => x.Company).ThenInclude(x => x.Entrepreneur);
            Entrepreneur entrepreneur = new Entrepreneur();
            foreach (var item in project)
            {
                entrepreneur = item.Company.Entrepreneur;
            }
            return entrepreneur;
        }
    }
}
