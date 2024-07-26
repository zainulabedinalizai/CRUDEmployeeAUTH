using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CRUDEmployeeAUTH.ActionFilters
{
    public class ExceptionHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = new ObjectResult(new { message = "An unexpected error occurred." })
            {
                StatusCode = 500
            };
            context.ExceptionHandled = true;
        }
    }
}