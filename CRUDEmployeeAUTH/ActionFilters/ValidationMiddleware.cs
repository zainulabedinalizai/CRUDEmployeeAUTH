//using CrudEmployeeAUTH.Models;
//using Microsoft.AspNetCore.Http;
//using System.Text.Json;
//using System.Threading.Tasks;

//public class ValidationMiddleware
//{
//    private readonly RequestDelegate _next;

//    public ValidationMiddleware(RequestDelegate next)
//    {
//        _next = next;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        if (context.Request.Method == HttpMethods.Post && context.Request.ContentType != null && context.Request.ContentType.Contains("application/json"))
//        {
//            var userRegister = await context.Request.ReadFromJsonAsync<UserRegister>();

//            if (userRegister != null)
//            {
//                var errors = new List<string>();
//                if (userRegister.Username.Length < 5)
//                {
//                    errors.Add("Username must be at least 5 characters long.");
//                }
//                if (userRegister.Password.Length < 8)
//                {
//                    errors.Add("Password must be at least 8 characters long.");
//                }

//                if (errors.Any())
//                {
//                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
//                    await context.Response.WriteAsJsonAsync(new { errors });
//                    return;
//                }
//            }
//        }
//        await _next(context);
//    }
//}


