using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDEmployeeAUTH.ActionFilters
{
    public class ResultFilter : IResultFilter
    {
        private readonly ILogger<ResultFilter> _logger;

        public ResultFilter(ILogger<ResultFilter> logger)
        {
            _logger = logger;
        }
        public void OnResultExecuting(ResultExecutingContext context)
        {
            _logger.LogInformation("ResultFilter - OnResultExecuting");
        }
        public void OnResultExecuted(ResultExecutedContext context)
        {
            _logger.LogInformation("ResultFilter - OnResultExecuted");
        }
    }
}