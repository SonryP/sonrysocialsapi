using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore;
using sonrysocialsapi.Models;
using sonrysocialsapi.Models.Responses;

namespace sonrysocialsapi.Infrastructure;

public class UserHandler : IUserHandler
{
    private readonly MineContext _context;

    public UserHandler(MineContext context)
    {
        _context = context;
    }

    public async Task<ActivationResponse> GenerateActivationToken(string username, string userUuid, string serverToken)
    {
        var findUser = await _context.Users.FirstOrDefaultAsync(u=> u.UUID == userUuid);
        var server = await _context.Servers.FirstOrDefaultAsync(s=>s.ApiKey == serverToken);
        if (findUser != null) return new ActivationResponse();
        if (server == null) return new ActivationResponse();
        User user = new User();
        user.UUID = userUuid;
        user.Username = username;
        user.Password = "NONE";
        user.Active = true;
        _context.Users.Add(user);
        
        Activation activation = new Activation();
        activation.ActivationToken = Guid.NewGuid().ToString(); 
        activation.User = user;
        activation.ActivationDate = DateTime.UtcNow;
        activation.ExpirationDate = DateTime.UtcNow.AddDays(15);
        activation.Server = server;
        activation.Active = true;
        _context.Activations.Add(activation);
        
        await _context.SaveChangesAsync();
        ActivationResponse response = new ActivationResponse();
        string token = activation.ActivationToken;
        response.ActivationToken  = token.Substring(0,token.Length-4);
        response.ActivationDigits = token.Substring(token.Length-4);
        return response;
    }

    public async Task<bool> ActivateUser(string activationToken, string username, string password)
    {
        var findActivation = await _context.Activations
            .Include(a=>a.User)
            .FirstOrDefaultAsync(a=>
                a.ActivationToken == activationToken && 
                a.User.Username == username && a.Active == true);
        if (findActivation != null)
        {
            findActivation.Active = false;
            User user = findActivation.User;
            string hash = Argon2.Hash(password);
            user.Password =  hash;
            user.Active = true;
            _context.Activations.Update(findActivation);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> AuthenticateUser(string username, string password)
    {
        var findUser = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));
        if (findUser == null) return false;
        return Argon2.Verify(findUser.Password,password);
    }

    public async Task<User> GetUser(string username)
    {
        var findUser = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));
        if (findUser == null) return null;
        return findUser;
    }

    public async Task<string> GetUserByPretoken(string pretoken)
    {
        var activation = await _context.Activations.Include(a=>a.User).FirstOrDefaultAsync(a => a.ActivationToken.Contains(pretoken) && a.Active);
        if (activation == null) return null;
        return activation.User.Username;
    }
    
}