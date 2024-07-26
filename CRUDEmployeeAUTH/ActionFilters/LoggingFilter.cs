using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDEmployeeAUTH.ActionFilters
{
    public class LoggingFilter : ActionFilterAttribute
    {
        private readonly ILogger<LoggingFilter> _logger;

        public LoggingFilter(ILogger<LoggingFilter> logger)
        {
            _logger = logger;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Action {ActionName} executing at {Time}", context.ActionDescriptor.DisplayName, DateTime.Now);
            base.OnActionExecuting(context);
        }
    }
}