using System.Runtime.CompilerServices;

namespace DevAuthServer.Constants;

public enum GrantType
{
    None = 0,
    AuthorizationCode,
    Implicit,
    Hybrid,

    // Some additional used for token endpoint
    // AuthorizationCode,
    RefreshToken,
    Password,
    ClientCredentials
}

public static class GrantTypeExtensions
{
    public static GrantType FromString(this GrantType grantType, string value)
    {
        return value switch
        {
            "authorization_code" => GrantType.AuthorizationCode,
            "implicit" => GrantType.Implicit,
            "hybrid" => GrantType.Hybrid,
            "refresh_token" => GrantType.RefreshToken,
            "password" => GrantType.Password,
            "client_credentials" => GrantType.ClientCredentials,
            _ => GrantType.None,
        };
    }

    public static GrantType FromResponseType(this GrantType grantType, string responseType)
    {
        return responseType switch
        {
            "code" => GrantType.AuthorizationCode,
            "id_token" => GrantType.Implicit,
            "id_token token" => GrantType.Implicit,
            "token" => GrantType.Implicit,
            "code token" => GrantType.Hybrid,
            "code id_token" => GrantType.Hybrid,
            "code id_token token" => GrantType.Hybrid,
            _ => GrantType.None,
        };
    }
}
