using Microsoft.EntityFrameworkCore;
using sonrysocialsapi.Models;

namespace sonrysocialsapi.Infrastructure;

public class TokenHandler: ITokenHandler
{
    private readonly MineContext _context;

    public TokenHandler(MineContext context)
    {
        _context = context;
    }

    public async Task<Server> ValidateToken(string token)
    {
        return await _context.Servers.FirstOrDefaultAsync(s => s.ApiKey == token && s.Active);
    }
}