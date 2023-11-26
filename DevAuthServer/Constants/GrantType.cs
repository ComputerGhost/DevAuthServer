using System.Runtime.CompilerServices;

namespace DevAuthServer.Constants;

public enum GrantType
{
    None = 0,
    AuthorizationCode,
    Implicit,
    Hybrid,
}

public static class GrantTypeExtensions
{
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
