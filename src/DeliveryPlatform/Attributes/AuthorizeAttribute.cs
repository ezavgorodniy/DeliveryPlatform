using System;
using Identity.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Shared.Interfaces;

namespace DeliveryPlatform.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly Role _expectedRole;

        public AuthorizeAttribute(Role expectedRole)
        {
            _expectedRole = expectedRole;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var executionContext = context.HttpContext.RequestServices.GetService<IExecutionContext>();

            if (!executionContext.IsInitialized ||
                !_expectedRole.HasFlag(executionContext.UserRole))
            {
                context.Result = new JsonResult(new {message = "Unauthorized"})
                    {StatusCode = StatusCodes.Status401Unauthorized};
            }
        }
    }
}
