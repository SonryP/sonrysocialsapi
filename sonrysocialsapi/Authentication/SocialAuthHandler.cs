using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using sonrysocialsapi.Infrastructure;
using sonrysocialsapi.Models;

namespace sonrysocialsapi.Authentication;

public class SocialAuthHandler:         
    AuthenticationHandler<SocialAuthOptions>
{
    private readonly ITokenHandler _tokenHandler;
    public SocialAuthHandler         
    (IOptionsMonitor<SocialAuthOptions> options, 
        ILoggerFactory logger, UrlEncoder encoder, 
        ISystemClock clock, ITokenHandler tokenHandler)
        : base(options, logger, encoder, clock) 
    { _tokenHandler = tokenHandler; }
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers 
                .ContainsKey(Options.TokenHeaderName))
        {
            return AuthenticateResult.Fail($"Missing header: {Options.TokenHeaderName}");
        }

        string token = Request 
            .Headers[Options.TokenHeaderName]!;

        Server server = await _tokenHandler.ValidateToken(token);
        
        if(server == null)
        {
            return AuthenticateResult
                .Fail($"Invalid token.");
        }
        
        var claims = new List<Claim>()
        {
            new Claim(server.Name,server.ApiKey)
        };

        var claimsIdentity = new ClaimsIdentity
            (claims, this.Scheme.Name);
        var claimsPrincipal = new ClaimsPrincipal 
            (claimsIdentity);

        return AuthenticateResult.Success
        (new AuthenticationTicket(claimsPrincipal, 
            this.Scheme.Name));
    }
}