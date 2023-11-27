using System.Runtime.CompilerServices;

namespace DevAuthServer.Constants;

public enum GrantType
{
    None,
    AuthorizationCode,
    ClientCredentials,
    Implicit,
    Password,
    RefreshToken,
}

public static class GrantTypeExtensions
{
    public static GrantType FromString(this GrantType _, string value)
    {
        return value switch
        {
            "authorization_code" => GrantType.AuthorizationCode,
            "client_credentials" => GrantType.ClientCredentials,
            "implicit" => GrantType.Implicit,
            "password" => GrantType.Password,
            "refresh_token" => GrantType.RefreshToken,
            _ => GrantType.None,
        };
    }
}
