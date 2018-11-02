using CrowdFunding.Data;
using CrowdFunding.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CrowdFunding.Authorization
{
    public class CheckProjectUserIdAuthorizationHandler : AuthorizationHandler<CheckProjectUserId, Project>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckProjectUserIdAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CheckProjectUserId requirement, Project resource)
        {
            //if (!context.User.HasClaim(c => c.Type == "Id" ))
            //    return Task.CompletedTask;



            var userId = _userManager.GetUserId(context.User);

            if (resource.Company.EntrepreneurId == userId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
