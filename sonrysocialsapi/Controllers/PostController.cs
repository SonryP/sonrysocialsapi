using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sonrysocialsapi.Infrastructure;
using sonrysocialsapi.Models;
using sonrysocialsapi.Models.Requests;

namespace sonrysocialsapi.Controllers;

[Authorize (AuthenticationSchemes = "Bearer")]
[Route("api/[controller]")]
[ApiController]
public class PostController: ControllerBase
{
    private IPostHandler _postHandler;

    public PostController(IPostHandler postHandler)
    {
        _postHandler = postHandler;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var identity = User.Identity as ClaimsIdentity;
        var username = identity.Claims.First(c => c.Type == "username").Value;
        var posts = await _postHandler.GetPosts(username);
        if (posts.Count == 0 || posts == null) return NoContent();
        return Ok(posts);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PostRequest post)
    {
        var identity = User.Identity as ClaimsIdentity;
        var username = identity.Claims.First(c => c.Type == "username").Value;
        Post _post = await _postHandler.CreatePost(post, username);
        if (_post == null) return BadRequest();
        return Ok(_post);
    }

    [HttpPost("Like")]
    public async Task<IActionResult> LikePost([FromQuery] int postId)
    {
        var identity = User.Identity as ClaimsIdentity;
        var username = identity.Claims.First(c => c.Type == "username").Value;
        var result = await _postHandler.LikePost(postId, username);
        if (!result) return BadRequest();
        return Ok(result);
    }
    
    [HttpPost("Unlike")]
    public async Task<IActionResult> UnLikePost([FromQuery] int postId)
    {
        var identity = User.Identity as ClaimsIdentity;
        var username = identity.Claims.First(c => c.Type == "username").Value;
        var result = await _postHandler.UnlikePost(postId, username);
        if (!result) return BadRequest();
        return Ok(result);
    }
    
}