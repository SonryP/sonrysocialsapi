using System.ComponentModel.DataAnnotations;

namespace sonrysocialsapi.Models.Requests;

public class PostRequest
{
    [Required]
    public string Content { get; set; }
    public string? ImageData { get; set; }
}