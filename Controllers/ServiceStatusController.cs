using Microsoft.AspNetCore.Mvc;
using PDLiSiteAPI.Models;
using PDLiSiteAPI.Services;
using System.Text.Json;

namespace PDLiSiteAPI.Controllers;

[Route("Api/[controller]")]
[ApiController]
public class ServiceStatusController : ControllerBase
{
    private readonly ILogger<ServiceStatusController> _logger;
    private readonly IServiceStatusService _serviceStatusService;

    public ServiceStatusController(
        ILogger<ServiceStatusController> logger,
        IServiceStatusService serviceStatusService
    )
    {
        _logger = logger;
        _serviceStatusService = serviceStatusService;
    }

    [HttpGet("{name}")]
    public ActionResult<ServiceStatus> Get(string name)
    {
        try
        {
            var res = _serviceStatusService.GetServiceStatus(name);
            _logger.LogInformation(
                "GET /Api/ServicesStatus/{name} {res}",
                name,
                JsonSerializer.Serialize(res)
            );
            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GET /Api/ServicesStatus/{name} ERROR", name);
            var err = new ServiceStatus()
            {
                Success = false,
                Name = name,
                Err = ex.Message
            };
            return new ObjectResult(err) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}
