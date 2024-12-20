using System.ComponentModel.DataAnnotations;

namespace sonrysocialsapi.Models;

public class Server
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string ApiKey { get; set; }
    public string IpAddress { get; set; }
    public bool Active { get; set; }
}