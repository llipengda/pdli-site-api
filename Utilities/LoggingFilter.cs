using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PDLiSiteAPI.Utilities;

public class LoggingFilter : IActionFilter
{
    private readonly ILogger<LoggingFilter> _logger;

    public LoggingFilter(ILogger<LoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var method = context.HttpContext.Request.Method;
        var path = context.HttpContext.Request.Path;
        var query = context.HttpContext.Request.QueryString;
        _logger.LogInformation("[REQUEST]  {method} {path}{query}", method, path, query);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var method = context?.HttpContext.Request.Method;
        var path = context?.HttpContext.Request.Path;
        var result = context?.Result as ObjectResult;
        var status = result?.StatusCode;
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        var jsonResult = JsonConvert.SerializeObject(result?.Value, settings);
        _logger.LogInformation(
            "[RESPONSE] {method} {path} {status}\n{result}",
            method,
            path,
            status,
            jsonResult
        );
    }
}
