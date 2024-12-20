using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sonrysocialsapi.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Username { get; set; }
    [JsonIgnore]
    public string Password { get; set; }
    public string UUID { get; set; }
    public bool Active { get; set; }
    
    
}