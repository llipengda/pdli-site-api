using Microsoft.AspNetCore.Mvc;
using PDLiSiteAPI.Services;
using System.ComponentModel.DataAnnotations;

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

    /// <summary>
    /// The method to authentication.
    /// Returns Ok(200) with a token if ok;
    /// BadRequest(400) if password is empty;
    /// Unauthoried(401) if password is incorrect.
    /// </summary>
    /// <param name="password">the password</param>
    /// <returns>token if success else error</returns>
    [HttpPost]
    public ActionResult<string> Login([Required] string password)
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
