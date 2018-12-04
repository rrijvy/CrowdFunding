using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFunding.Services
{
    public interface IContactEmailSender
    {
        Task SendContactEmailAsync(string email, string subject, string htmlMessage);
    }
}
