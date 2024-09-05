using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

public class RoleBasedAuthorizationFilter : IAsyncActionFilter
{
    private readonly string _role;
    private readonly string _company;

    public RoleBasedAuthorizationFilter(string role, string company = null)
    {
        _role = role;
        _company = company;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var user = context.HttpContext.User;
        var userRole = user.FindFirst("Role")?.Value;
        var userCompany = user.FindFirst("Company")?.Value;

        if (userRole == null || (userRole != _role && userRole != "SystemAdmin"))
        {
            context.Result = new ForbidResult();
            return;
        }

        if (_company != null && userRole == "CompanyAdmin" && userCompany != _company)
        {
            context.Result = new ForbidResult();
            return;
        }

        await next();
    }
}
