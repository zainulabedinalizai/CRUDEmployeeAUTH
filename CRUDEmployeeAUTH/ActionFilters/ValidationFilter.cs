using CrudEmployeeAUTH.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDEmployeeAUTH.ActionFilters
{
    public class ValidationFilter : IActionFilter
    {
        private readonly ILogger<ValidationFilter> _logger;

        public ValidationFilter(ILogger<ValidationFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.Values.OfType<UserRegister>().Any())
            {
                var userRegister = context.ActionArguments.Values.OfType<UserRegister>().FirstOrDefault();

                if (userRegister != null)
                {
                    bool hasErrors = false;
                    if (userRegister.Username.Length < 5)
                    {
                        var usernameError = "Username must be at least 5 characters long.";
                        context.ModelState.AddModelError("Username", usernameError);
                        _logger.LogWarning(usernameError); 
                        hasErrors = true;
                    }
                    if (userRegister.Password.Length < 8)
                    {
                        var passwordError = "Password must be at least 8 characters long.";
                        context.ModelState.AddModelError("Password", passwordError);
                        _logger.LogWarning(passwordError); 
                        hasErrors = true;
                    }
                    if (hasErrors)
                    {
                        context.Result = new BadRequestObjectResult(context.ModelState);
                    }
                }
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}