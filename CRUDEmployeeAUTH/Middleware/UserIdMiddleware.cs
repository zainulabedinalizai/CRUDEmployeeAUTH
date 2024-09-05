public class UserIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UserIdMiddleware> _logger;

    public UserIdMiddleware(RequestDelegate next, ILogger<UserIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userId = GetUserIdFromHeader(context);
        if (!string.IsNullOrEmpty(userId))
        {
            _logger.LogInformation("UserId found in header: {UserId}", userId);
            context.Items["UserId"] = userId;
        }
        else
        {
            _logger.LogWarning("UserId not found in header.");
        }
        await _next(context);
    }

    private string GetUserIdFromHeader(HttpContext context)
    {
        return context.Request.Headers["UserId"].ToString();
    }
}