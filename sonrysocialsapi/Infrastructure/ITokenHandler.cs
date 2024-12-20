using sonrysocialsapi.Models;

namespace sonrysocialsapi.Infrastructure;

public interface ITokenHandler
{ 
    Task<Server> ValidateToken(string token);
}