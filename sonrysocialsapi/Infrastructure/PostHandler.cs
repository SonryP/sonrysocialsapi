using Microsoft.EntityFrameworkCore;
using sonrysocialsapi.Models;
using sonrysocialsapi.Models.Requests;

namespace sonrysocialsapi.Infrastructure;

public class PostHandler : IPostHandler
{
    private readonly MineContext _context;

    public PostHandler(MineContext context)
    {
        _context = context;
    }

    public async Task<Post> CreatePost(PostRequest post, string username)
    {
        var findUser = await _context.Users.FirstOrDefaultAsync(u=>u.Username == username);
        if (findUser == null) return null;
        Post _post = new Post();
        _post.Content = post.Content;
        if (!string.IsNullOrEmpty(post.ImageData))
        {
            var imgOffset = post.ImageData.IndexOf(',') + 1;
            _post.Attachment = System.Convert.FromBase64String(post.ImageData[imgOffset..^0]);
        }
        _post.User = findUser;
        _post.Likes = 0;
        _post.Active = true;
        _post.Created = DateTime.UtcNow;
        _context.Posts.Add(_post);
        await _context.SaveChangesAsync();
        return _post;
    }

    public async Task<List<Post>> GetPosts()
    {
        return await _context.Posts.Include(p=> p.User).Where(p=>p.Active).OrderByDescending(p=>p.Created).ToListAsync();
    }

    public async Task<bool> DeletePost(int postId)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null) return false;
        post.Active = false;
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> LikePost(int postId, int userId)
    {
        var post = await _context.Posts.Include(p=>p.LikesList).FirstOrDefaultAsync(p=>p.Id==postId);
        if (post == null) return false;
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;
        bool userAlreadyLiked = post.LikesList.Any(like => like.User.Id == userId);
        if (userAlreadyLiked) return false;
        
        Like like = new Like();
        like.IsLiked = true;
        like.User = user;
        like.Liked = DateTime.UtcNow;
        post.LikesList.Add(like);
        post.Likes++;
        _context.Likes.Add(like);
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnlikePost(int postId, int userId)
    {
        var post = await _context.Posts
            .Include(p=>p.User)
            .Include(p=>p.LikesList)
            .FirstOrDefaultAsync(p=>p.Id == postId);
        if (post == null) return false;
        if (post.User == null) return false;
        var like = post.LikesList.FirstOrDefault(l=>l.User.Id == post.User.Id && l.IsLiked);
        if (like == null) return false;
        post.Likes--;
        like.IsLiked = false;
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
        return true;
    }
}