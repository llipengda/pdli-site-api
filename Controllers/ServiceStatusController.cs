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

    [HttpGet]
    public ActionResult<ServiceStatus> Get(string name)
    {
        return _serviceStatusService.GetServiceStatus(name);
    }
}
