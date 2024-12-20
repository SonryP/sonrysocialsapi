using sonrysocialsapi.Models;
using sonrysocialsapi.Models.Responses;

namespace sonrysocialsapi.Infrastructure;

public interface IUserHandler
{
    Task<ActivationResponse> GenerateActivationToken(string username, string userUuid, string serverToken);
    Task<bool> ActivateUser(string activationToken, string username, string password);
    Task<bool> AuthenticateUser(string username, string password);
    Task<User> GetUser(string username);
    Task<string> GetUserByPretoken(string pretoken);

}