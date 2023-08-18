using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PDLiSiteAPI.Services;

public interface ILoginService
{
    string Generate();
}

public class LoginService : ILoginService
{
    private readonly IConfiguration _configuration;

    public LoginService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Generate()
    {
        var algorithm = SecurityAlgorithms.HmacSha256;

        var claims = new[] { new Claim(ClaimTypes.Role, "Admin") };

        var secretKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])
        );

        var signingCredentials = new SigningCredentials(secretKey, algorithm);

        var jwtSecurityToken = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            DateTime.Now,
            DateTime.Now.AddDays(30),
            signingCredentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return token;
    }
}
