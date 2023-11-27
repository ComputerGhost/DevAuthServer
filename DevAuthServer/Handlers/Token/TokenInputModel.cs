using DevAuthServer.Constants;
using DevAuthServer.Storage;

namespace DevAuthServer.Handlers.Token;

public class TokenInputModel
{
    public string grant_type { get; set; } = null!;

    // Used for authorization_code grant.
    public string? code { get; set; }

    // Used for refresh_token grant.
    public string? refresh_token { get; set; }

    // Used for password_grant.
    public string? username { get; set; }
    public string? password { get; set; }
    public string? scope { get; set; } // Also used for client_credentials.

    // We're ignoring these.
    public string client_id { get; set; } = null!;
    public string? client_secret { get; set; }
    public string? redirect_uri { get; set; }
    public string? code_verifier { get; set; }

    public void Validate(Database database)
    {
        switch (new GrantType().FromString(grant_type))
        {
            case GrantType.AuthorizationCode: 
                Validate_AuthorizationCode(database); 
                break;
            case GrantType.ClientCredentials:
                Validate_ClientCredentials();
                break;
            case GrantType.Password:
                Validate_Password(database);
                break;
            case GrantType.Implicit:
                Validate_Implicit();
                break;
            case GrantType.RefreshToken: 
                Validate_RefreshToken(database); 
                break;
            default:
                throw new Exception("grant_type is not valid.");
        }
    }

    public void Validate_AuthorizationCode(Database database)
    {
        if (code == null)
            throw new Exception("code is required for authorization_code grant.");
        if (!database.AuthorizationCodes.Any(c => c.code == code))
            throw new Exception("code does not exist.");
    }

    public void Validate_ClientCredentials()
    {
        if (scope == null)
            throw new Exception("scope is required for client_credentials grant.");
    }

    public void Validate_Implicit()
    {
        throw new Exception("Implicit grant_type should not use the token endpoint.");
    }

    public void Validate_Password(Database database)
    {
        if (username == null)
            throw new Exception("username is required for password grant.");
        if (password == null)
            throw new Exception("password is required for password grant.");
        if (!database.Users.Any(u => u.Email == username))
            throw new Exception("User does not exist.");
    }

    public void Validate_RefreshToken(Database database)
    {
        if (refresh_token == null)
            throw new Exception("refresh_token is required for refresh_token grant.");
        if (!database.AccessTokens.Any(t => t.refresh_token == refresh_token))
            throw new Exception("refresh_token does not exist.");
    }
}
