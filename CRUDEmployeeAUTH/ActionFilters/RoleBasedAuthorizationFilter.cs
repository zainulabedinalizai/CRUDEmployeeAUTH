using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDEmployeeAUTH.ActionFilters
{
    public class RoleBasedAuthorizationFilter : IAuthorizationFilter
    {
        private readonly string _requiredRole;

        public RoleBasedAuthorizationFilter(string requiredRole)
        {
            _requiredRole = requiredRole;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                var companyClaim = user.FindFirst("Company")?.Value;
                if (companyClaim != _requiredRole)
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}