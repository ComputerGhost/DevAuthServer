using System.Runtime.CompilerServices;

namespace DevAuthServer.Constants;

public enum GrantType
{
    None,
    RefreshToken,
    Password,
    ClientCredentials
}

public static class GrantTypeExtensions
{
    public static GrantType FromString(this GrantType _, string value)
    {
        return value switch
        {
            "refresh_token" => GrantType.RefreshToken,
            "password" => GrantType.Password,
            "client_credentials" => GrantType.ClientCredentials,
            _ => GrantType.None,
        };
    }
}
