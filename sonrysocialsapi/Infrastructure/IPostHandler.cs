using sonrysocialsapi.Models;
using sonrysocialsapi.Models.Requests;

namespace sonrysocialsapi.Infrastructure;

public interface IPostHandler
{
    Task<List<Post>> GetPosts();
    Task<Post> CreatePost(PostRequest post, string username);
    Task<bool> DeletePost(int postId);
    Task<bool> LikePost(int postId, int userId);
    Task<bool> UnlikePost(int postId, int userId);
    
}