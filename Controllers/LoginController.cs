using Microsoft.AspNetCore.Mvc;
using PDLiSiteAPI.Services;

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

    [HttpPost]
    public ActionResult<string> Login(string password)
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
