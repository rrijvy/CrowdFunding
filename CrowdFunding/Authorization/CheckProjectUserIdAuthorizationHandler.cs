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
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CheckProjectUserId requirement, Project resource)
        {
            if (!context.User.HasClaim(c => c.Type == "Id" ))
                return Task.CompletedTask;

            string userId = context.User.FindFirst(c => c.Type == "Id").Value;

            if (resource.Company.EntrepreneurId == userId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
