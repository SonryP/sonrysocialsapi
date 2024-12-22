using sonrysocialsapi.Models;
using sonrysocialsapi.Models.Requests;

namespace sonrysocialsapi.Infrastructure;

public interface IPostHandler
{
    Task<List<Post>> GetPosts(string username);
    Task<Post> CreatePost(PostRequest post, string username);
    Task<bool> DeletePost(int postId);
    Task<bool> LikePost(int postId, string username);
    Task<bool> UnlikePost(int postId, string username);
    
}