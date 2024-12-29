using System.Security.Cryptography;
using System.Text;
using System.Web;
using Microsoft.EntityFrameworkCore;
using sonrysocialsapi.Models;
using sonrysocialsapi.Models.Requests;
using sonrysocialsapi.Models.Responses;

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
        var findUser = await _context.Users.FirstOrDefaultAsync(u=>u.Username.ToLower().Equals(username.ToLower()));
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

    public async Task<List<Post>> GetPosts(string username)
    {
        var posts = await _context.Posts
            .Include(p => p.User)
            .Include(p => p.LikesList) 
            .Where(p => p.Active)
            .OrderByDescending(p => p.Created)
            .ToListAsync();

        foreach (var post in posts)
        {
            post.LikedByUser = post.LikesList.Any(like => like.User.Username == username && like.IsLiked);
        }

        return posts;
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

    public async Task<bool> LikePost(int postId, string username)
    {
        var post = await _context.Posts.Include(p=>p.LikesList).ThenInclude(l => l.User ).FirstOrDefaultAsync(p=>p.Id==postId);
        if (post == null) return false;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));
        if (user == null) return false;
        bool userAlreadyLiked = post.LikesList.Any(like => like.User.Id == user.Id && like.IsLiked);
        if (userAlreadyLiked) return false;
        Like liked = post.LikesList.FirstOrDefault(like => like.User.Id == user.Id && !like.IsLiked);
        if (liked != null)
        {
            liked.IsLiked = true;
            post.Likes++;
            _context.Posts.Update(post);
            _context.Likes.Update(liked);
            await _context.SaveChangesAsync();
            return true;
        }
        
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

    public async Task<bool> UnlikePost(int postId, string username)
    {
        var post = await _context.Posts
            .Include(p=>p.User)
            .Include(p=>p.LikesList)
            .ThenInclude(l=>l.User)
            .FirstOrDefaultAsync(p=>p.Id == postId);
        if (post == null) return false;
        if (post.User == null) return false;
        var like = post.LikesList.FirstOrDefault(l=>l.User.Username.ToLower().Equals(username.ToLower()) && l.IsLiked);
        if (like == null) return false;
        post.Likes--;
        like.IsLiked = false;
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Post> GetPost(string postId)
    {
        var share = await _context.Shares.Include(s=>s.Post).ThenInclude(p=>p.User).Include(s=>s.Post).ThenInclude(p=>p.LikesList).Where(s=>s.ShareHash == postId).FirstOrDefaultAsync();
        if (share == null) return new Post();
        if (share.Post == null) return new Post();
        return share.Post;
    }

    public async Task<ShareResponse> SharePost(int postId, string username)
    {
        var post = await _context.Posts.Include(p=>p.User).FirstOrDefaultAsync(p=>p.Id==postId);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));
        if (post == null) return new ShareResponse();
        if (user == null) return new ShareResponse();
        if (post.User != user) return new ShareResponse();
        var share = await _context.Shares.Include(s=>s.Post).Where(s=>s.Post == post).FirstOrDefaultAsync();
        if (share != null)
        {
            return new ShareResponse(){ShareHash = share.ShareHash};
        }
        string hash = GeneratePostShareHash(post.Content.ToLower(), username);
        if (string.IsNullOrEmpty(hash)) return new ShareResponse();
        Share sharePost = new Share();
        sharePost.Post = post;
        sharePost.ShareHash = hash;
        sharePost.Created = DateTime.UtcNow;
        sharePost.Active = true;
        _context.Shares.Add(sharePost);
        await _context.SaveChangesAsync();
        return new ShareResponse() { ShareHash = hash };
    }
    
    private string GeneratePostShareHash(string postId, string salt)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt)))
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(postId));
            return Convert.ToBase64String(hash);
        }
    }
}