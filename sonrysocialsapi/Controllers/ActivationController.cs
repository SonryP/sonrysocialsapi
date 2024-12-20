using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sonrysocialsapi.Infrastructure;
using sonrysocialsapi.Models.Requests;

namespace sonrysocialsapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActivationController : ControllerBase
{
    private IUserHandler _userHandler;

    public ActivationController(IUserHandler userHandler)
    {
        _userHandler = userHandler;
    }
    
    [Authorize (AuthenticationSchemes = "SocialAuthScheme")]
    [HttpPost("GetActivationCode")]
    public async Task<IActionResult> GetActivationCode(ActivationRequest request)
    {
        var identity = User.Identity as ClaimsIdentity;
        string serverId = identity.Claims.FirstOrDefault().Value;
        var activationData = _userHandler.GenerateActivationToken(request.Username, request.Uuid, serverId);
        if(activationData == null) return Unauthorized();
        return Ok(activationData);
    }

    [HttpPost("Activate")]
    public async Task<IActionResult> Activate(ActivateUserRequest request)
    {
        var activation = await _userHandler.ActivateUser(request.ActivationToken, request.Username, request.Password);
        if(!activation) return Unauthorized();
        return Ok(activation);
    }
    
}