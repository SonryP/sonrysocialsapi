using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sonrysocialsapi.Models;

public class Like
{
    [Key]
    public int Id { get; set; }
    public User User { get; set; }
    [JsonIgnore]
    public DateTime Liked { get; set; }
    public bool IsLiked { get; set; }
}