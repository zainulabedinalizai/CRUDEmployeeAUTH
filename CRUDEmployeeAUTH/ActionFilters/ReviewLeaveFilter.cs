using CRUDEmployeeAUTH.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDEmployeeAUTH.ActionFilters
{
    public class ReviewLeaveFilter : IAsyncAuthorizationFilter
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IUserRepository _userRepository;

        public ReviewLeaveFilter(ILeaveRepository leaveRepository, IUserRepository userRepository)
        {
            _leaveRepository = leaveRepository;
            _userRepository = userRepository;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!context.RouteData.Values.TryGetValue("leaveId", out var leaveIdValue) || !int.TryParse(leaveIdValue.ToString(), out var leaveId))
            {
                context.Result = new BadRequestObjectResult("Invalid LeaveId provided.");
                return;
            }

            var query = context.HttpContext.Request.Query;
            var companyName = query["companyName"].ToString();

            if (string.IsNullOrEmpty(companyName))
            {
                context.Result = new BadRequestObjectResult("Company name is required in the query string.");
                return;
            }

            var companyIdClaim = user.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyIdClaim))
            {
                context.Result = new BadRequestObjectResult("User does not have a valid CompanyId claim.");
                return;
            }

            var leave = await _leaveRepository.GetLeaveByIdAsync(leaveId);
            var company = await _userRepository.GetCompanyByNameAsync(companyName);

            if (leave == null)
            {
                context.Result = new NotFoundObjectResult("Leave record not found.");
                return;
            }

            if (leave.User == null)
            {
                context.Result = new NotFoundObjectResult("Leave record does not have an associated user.");
                return;
            }

            if (company == null)
            {
                context.Result = new NotFoundObjectResult("Company not found.");
                return;
            }

            if (user.IsInRole("SystemAdmin"))
            {
                if (leave.User.CompanyId != company.CompanyId)
                {
                    context.Result = new BadRequestObjectResult("The leave does not belong to the specified company.");
                    return;
                }
                return;
            }

            if (user.IsInRole("CompanyAdmin"))
            {
                if (leave.User.CompanyId.ToString() != companyIdClaim || company.CompanyId.ToString() != companyIdClaim)
                {
                    context.Result = new BadRequestObjectResult("User does not belong to the specified company.");
                    return;
                }
            }
            else
            {
                context.Result = new BadRequestObjectResult("You do not have the necessary permissions to review this leave.");
            }
        }
    }
}