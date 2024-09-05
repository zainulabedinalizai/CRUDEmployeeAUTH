using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class UserIdHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var actionAttributes = context.MethodInfo.GetCustomAttributes(true);
        var isGetUserById = actionAttributes.Any(attr => attr.GetType() == typeof(HttpGetAttribute) && ((HttpGetAttribute)attr).Template == "getuserbyid");
       


        if (isGetUserById )
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "UserId",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                },
                Description = "Please enter the UserId"
            });
        }
    }
}