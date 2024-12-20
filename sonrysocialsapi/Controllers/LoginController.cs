using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using sonrysocialsapi.Infrastructure;
using sonrysocialsapi.Models.Requests;
using sonrysocialsapi.Models.Responses;

namespace sonrysocialsapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private IConfiguration _config;
    private IUserHandler _userHandler;
    public LoginController(IConfiguration config, IUserHandler userHandler) 
    {
        _config = config;
        _userHandler = userHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] LoginRequest request)
    {
        var auth = await _userHandler.AuthenticateUser(request.Username, request.Password);
        
        if (!auth) return Unauthorized();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Issuer"],
            new Claim[]{new Claim("username", request.Username),},
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        var token =  new JwtSecurityTokenHandler().WriteToken(Sectoken);
        var user = await _userHandler.GetUser(request.Username);
        TokenResponse response = new();
        response.Token = token;
        response.Username = user.Username;
        response.UserId = user.UUID;
        return Ok(response);
    }
}