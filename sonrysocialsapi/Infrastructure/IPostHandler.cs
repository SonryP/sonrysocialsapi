using sonrysocialsapi.Models;
using sonrysocialsapi.Models.Requests;
using sonrysocialsapi.Models.Responses;

namespace sonrysocialsapi.Infrastructure;

public interface IPostHandler
{
    Task<List<Post>> GetPosts(string username, int page, int pageSize);
    Task<Post> CreatePost(PostRequest post, string username);
    Task<bool> DeletePost(int postId);
    Task<bool> LikePost(int postId, string username);
    Task<bool> UnlikePost(int postId, string username);
    Task<Post> GetPost(string postId);
    Task<ShareResponse> SharePost(int postId, string username);

}