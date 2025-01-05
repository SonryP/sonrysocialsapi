using System.ComponentModel.DataAnnotations;

namespace sonrysocialsapi.Models;

public class Share
{
    [Key]
    public int Id { get; set; }
    public string ShareHash { get; set; }
    public Post Post { get; set; }
    public DateTime Created { get; set; }
    public bool Active { get; set; }
    public int AccessCount { get; set; }
}