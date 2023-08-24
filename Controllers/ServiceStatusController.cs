using Microsoft.AspNetCore.Mvc;
using PDLiSiteAPI.Models;
using PDLiSiteAPI.Services;
using System.ComponentModel.DataAnnotations;

namespace PDLiSiteAPI.Controllers;

[Route("Api/[controller]/[action]")]
[ApiController]
public class ServiceStatusController : ControllerBase
{
    private readonly IServiceStatusService _serviceStatusService;

    public ServiceStatusController(IServiceStatusService serviceStatusService)
    {
        _serviceStatusService = serviceStatusService;
    }

    /// <summary>
    /// Get status of a service
    /// </summary>
    /// <param name="name">name of the service</param>
    /// <response code="200">Ok</response>
    /// <returns>a <see cref="ServiceStatus"/> object</returns>
    [HttpGet]
    public async Task<ActionResult<ServiceStatus>> Get([Required] string name)
    {
        return await _serviceStatusService.GetServiceStatusAsync(name);
    }
}
