using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace CRUDEmployeeAUTH.ActionFilters
{
    public class CacheFilter : IAsyncActionFilter
    {
        private readonly int _duration;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheFilter> _logger;

        public CacheFilter(int duration, IMemoryCache cache, ILogger<CacheFilter> logger)
        {
            _duration = duration;
            _cache = cache;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            _logger.LogInformation($"Checking cache for key: {cacheKey}");

            if (_cache.TryGetValue(cacheKey, out var cachedValue))
            {
                _logger.LogInformation($"Cache hit for key: {cacheKey}");
                var contentResult = new ContentResult
                {
                    Content = cachedValue.ToString(),
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

            _logger.LogInformation($"Cache miss for key: {cacheKey}");
            var executedContext = await next();

            if (executedContext.Result is ObjectResult objectResult)
            {
                var result = objectResult.Value.ToString();
                _cache.Set(cacheKey, result, TimeSpan.FromSeconds(_duration));
                _logger.LogInformation($"Cached result for key: {cacheKey}");
            }
        }
        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var builder = new StringBuilder();
            builder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                builder.Append($"|{key}-{value}");
            }
            return builder.ToString();
        }
    }
}