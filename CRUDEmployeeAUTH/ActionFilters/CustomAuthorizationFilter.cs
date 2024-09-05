using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class CustomAuthorizationFilter : IAsyncActionFilter
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<CustomAuthorizationFilter> _logger;

    public CustomAuthorizationFilter(IUserRepository userRepository, ILogger<CustomAuthorizationFilter> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var actionDescriptor = context.ActionDescriptor;
        var httpContext = context.HttpContext;

        // Determine the specific route being accessed
        if (actionDescriptor.AttributeRouteInfo.Template.Contains("company") ||
            actionDescriptor.AttributeRouteInfo.Template.Contains("getuserbyid"))
        {
            await HandleCompanyOrUserByIdRequest(context, httpContext);
        }
        else if (actionDescriptor.AttributeRouteInfo.Template.Contains("all") && httpContext.User.IsInRole("SystemAdmin"))
        {
            await HandleGetAllRequest(context);
        }
        else if (actionDescriptor.AttributeRouteInfo.Template.Contains("profile") ||
                 actionDescriptor.AttributeRouteInfo.Template.Contains("employee-profile"))
        {
            await HandleProfileRequest(context, httpContext);
        }

        if (context.Result == null)
        {
            await next();
        }
    }
    private async Task HandleCompanyOrUserByIdRequest(ActionExecutingContext context, HttpContext httpContext)
    {
        var query = httpContext.Request.Query;
        var companyName = query["companyName"].ToString();

        _logger.LogInformation("Company Name: {companyName}", companyName);

        if (string.IsNullOrEmpty(companyName))
        {
            context.Result = new BadRequestObjectResult("Company name must be provided.");
            return;
        }

        var companyIdClaim = httpContext.User.FindFirst("CompanyId")?.Value;
        var userIdFromMiddleware = httpContext.Items["UserId"] as string;

        if (string.IsNullOrEmpty(userIdFromMiddleware))
        {
            context.Result = new BadRequestObjectResult("UserId not found in headers.");
            return;
        }

        if (!int.TryParse(userIdFromMiddleware, out var userId))
        {
            context.Result = new BadRequestObjectResult("Invalid UserId.");
            return;
        }

        if (httpContext.User.IsInRole("SystemAdmin"))
        {
            await HandleSystemAdminRequest(context, companyName, userId);
        }
        else if (string.IsNullOrEmpty(companyIdClaim))
        {
            context.Result = new BadRequestObjectResult("Company claim not found.");
        }
        else
        {
            await HandleCompanyAdminRequest(context, companyName, userId, companyIdClaim);
        }
    }
    private async Task HandleSystemAdminRequest(ActionExecutingContext context, string companyName, int userId)
    {
        var targetCompany = await _userRepository.GetCompanyByNameAsync(companyName);
        if (targetCompany == null)
        {
            context.Result = new NotFoundObjectResult("Company not found.");
            return;
        }

        await HandleUserById(context, userId, targetCompany.CompanyId);
    }

    private async Task HandleCompanyAdminRequest(ActionExecutingContext context, string companyName, int userId, string companyIdClaim)
    {
        if (!int.TryParse(companyIdClaim, out var companyId))
        {
            context.Result = new BadRequestObjectResult("Invalid Company claim.");
            return;
        }

        var company = await _userRepository.GetCompanyByNameAsync(companyName);
        if (company == null || company.CompanyId != companyId)
        {
            context.Result = new UnauthorizedObjectResult("You are not authorized.");
            return;
        }

        await HandleUserById(context, userId, companyId);
    }

    private async Task HandleUserById(ActionExecutingContext context, int userId, int companyId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null || user.CompanyId != companyId)
        {
            context.Result = new NotFoundObjectResult("User not found or does not belong to the company.");
            return;
        }
        context.HttpContext.Items["User"] = user;
    }

    private async Task HandleGetAllRequest(ActionExecutingContext context)
    {
        var allUsers = await _userRepository.GetAllAsync();
        if (allUsers == null || !allUsers.Any())
        {
            context.Result = new NotFoundObjectResult("No users found.");
            return;
        }
        context.HttpContext.Items["CompanyUsers"] = allUsers;
    }

    private async Task HandleProfileRequest(ActionExecutingContext context, HttpContext httpContext)
    {
        var email = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(email))
        {
            context.Result = new BadRequestObjectResult("User email claim not found.");
            return;
        }

        var user = await _userRepository.GetByUsernameAsync(email);
        if (user == null)
        {
            context.Result = new NotFoundObjectResult("User not found.");
            return;
        }
        context.HttpContext.Items["UserProfile"] = user;
    }
}