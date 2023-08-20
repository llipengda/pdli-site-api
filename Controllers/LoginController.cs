using Microsoft.AspNetCore.Mvc;
using PDLiSiteAPI.Services;
using System.ComponentModel.DataAnnotations;

namespace PDLiSiteAPI.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _configuration;

    private readonly ILoginService _loginService;

    public LoginController(IConfiguration configuration, ILoginService loginService)
    {
        _configuration = configuration;
        _loginService = loginService;
    }

    /// <summary>
    /// Authentication
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
            return BadRequest("Password can't be empty");
        }
        if (password.Trim() != _configuration["Password"])
        {
            return Unauthorized("The password is incorrect, please try again");
        }
        var token = _loginService.Generate();
        return Ok(token);
    }
}
