using System.ComponentModel.DataAnnotations;

namespace sonrysocialsapi.Models;

public class Like
{
    [Key]
    public int Id { get; set; }
    public User User { get; set; }
    public DateTime Liked { get; set; }
    public bool IsLiked { get; set; }
}