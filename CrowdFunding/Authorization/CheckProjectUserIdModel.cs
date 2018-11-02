using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFunding.Authorization
{
    public class CheckProjectUserIdModel
    {
        public int ProjectId { get; set; }
        public string EntreprenuerId { get; set; }
    }
}
