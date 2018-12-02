using CrowdFunding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFunding.Services
{
    public interface IGetProjectOwner
    {
        Entrepreneur GetOwner(int projectId);
    }
}
