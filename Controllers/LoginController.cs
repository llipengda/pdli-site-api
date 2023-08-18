using Microsoft.AspNetCore.Mvc;
using PDLiSiteAPI.Services;

namespace PDLiSiteAPI.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;

    private readonly IConfiguration _configuration;

    private readonly ILoginService _loginService;

    public LoginController(
        ILogger<LoginController> logger,
        IConfiguration configuration,
        ILoginService loginService
    )
    {
        _logger = logger;
        _configuration = configuration;
        _loginService = loginService;
    }

    [HttpPost]
    public ActionResult<string> Login(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            _logger.LogWarning(
                "POST /Api/Login?password={password} ERROR Password can't be empty",
                password
            );
            return BadRequest("Password can't be empty");
        }
        if (password.Trim() != _configuration["Password"])
        {
            _logger.LogWarning(
                "POST /Api/Login?password={password} ERROR INCORRECT PASSWORD",
                password
            );
            return Unauthorized("INCORRECT PASSWORD");
        }
        var token = _loginService.Generate();
        _logger.LogInformation("POST /Api/Login?password={password} {token}", password, token);
        return Ok(token);
    }
}
