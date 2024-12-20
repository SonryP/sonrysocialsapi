namespace sonrysocialsapi.Models.Requests;

public class ActivateUserRequest
{
    public string ActivationToken { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}