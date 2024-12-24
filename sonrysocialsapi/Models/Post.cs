using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace sonrysocialsapi.Models;

public class Post
{
    [Key]
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime Created { get; set; }
    public int Likes { get; set; }
    public User User { get; set; }
    public List<Like> LikesList { get; set; }
    public byte[]? Attachment { get; set; }
    public bool Active { get; set; }
    [NotMapped]
    public bool LikedByUser { get; set; }
}