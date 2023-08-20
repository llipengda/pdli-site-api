using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PDLiSiteAPI.Utilities;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var method = context.HttpContext.Request.Method;
        var path = context.HttpContext.Request.Path;
        var query = context.HttpContext.Request.QueryString;
        _logger.LogError(
            context.Exception,
            "[REQUEST]  {method} {path}{query}",
            method,
            path,
            query
        );
        context.Result = new ObjectResult(new { Success = false, Err = context.Exception.Message })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}
