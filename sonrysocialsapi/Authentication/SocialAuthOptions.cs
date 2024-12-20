using Microsoft.AspNetCore.Authentication;

namespace sonrysocialsapi.Authentication;

public class SocialAuthOptions:         
    AuthenticationSchemeOptions
{
    public const string DefaultScheme = "SocialAuthScheme";
    public string TokenHeaderName { get; set; } = "ApiToken";
}