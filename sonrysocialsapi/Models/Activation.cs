using System.ComponentModel.DataAnnotations;

namespace sonrysocialsapi.Models;

public class Activation
{
    [Key]
    public int Id { get; set; }
    public string ActivationToken { get; set; }
    public DateTime ActivationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool Active { get; set; }
    public User User { get; set; }
    public Server Server { get; set; }
}