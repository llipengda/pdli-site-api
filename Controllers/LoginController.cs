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
    /// </summary>
    /// <param name="password">password - example: "123456"</param>
    /// <response code="200">ok - returns the token</response>
    /// <response code="400">the password is empty or not a string</response>
    /// <response code="401">the password is incorrect</response>
    /// <returns>token if success else error</returns>
    [HttpPost]
    public ActionResult<string> Login([FromBody] [Required] string password)
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
            return Unauthorized("The password is incorrect, please try again");
        }
        var token = _loginService.Generate();
        _logger.LogInformation("POST /Api/Login?password={password} {token}", password, token);
        return Ok(token);
    }
}
