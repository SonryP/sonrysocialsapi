using System.Security.Claims;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sonrysocialsapi.Infrastructure;
using sonrysocialsapi.Models;

namespace sonrysocialsapi.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly IUserHandler _userHandler;
    private readonly IPostHandler _postHandler;

    public TestController(IUserHandler userHandler, IPostHandler postHandler)
    {
        _userHandler = userHandler;
        _postHandler = postHandler;
    }
    
    [Authorize (AuthenticationSchemes = "Bearer,SocialAuthScheme")]
    // GET
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery]string password)
    {  
        //Activation Process
        // var identity = User.Identity as ClaimsIdentity;
        // string serverId = identity.Claims.FirstOrDefault().Value;
        // var activation = await _userHandler.GenerateActivationToken("Sonry_tm","435b96eebb5449be8225923e4c0126ab", serverId);
        // return Ok(activation);

        //Activation with token
        //var check = await _userHandler.ActivateUser("7d0ff296-2b8b-4aa6-948a-4c4dce41b706", "Sonry_tm", "123123hh");
        
        
        //check auth
        //var auth = await _userHandler.AuthenticateUser("Sonry_tm", "123123hh");
        var user = await _userHandler.GetUser("Sonry_tm");
        //Create post
        //        Post post = new Post();
        // if (user != null)
        // {
        //     
        //     post.Active = true;
        //     post.User = user;
        //     post.Content = "Hello World!";
        //     post.Created = DateTime.UtcNow;
        //     post.Likes = 0;
        //     await _postHandler.CreatePost(post);
        // }

        //var liked = await _postHandler.LikePost(1, user.Id);
        //var unliked = await _postHandler.UnlikePost(1,user.Id);
        
        return Ok("Holi!");
    }
}