using DevAuthServer.Constants;
using DevAuthServer.Storage;

namespace DevAuthServer.Handlers.Token;

public class TokenInputModel
{
    public string grant_type { get; set; } = null!;
    public string code { get; set; } = null!;
    public string? redirect_uri { get; set; }

    public string client_id { get; set; }
    public string client_secret { get; set; }
    public string state { get; set; }
    public string refresh_token { get; set; }
    public string code_verifier { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public string at_hash { get; set; }

    public void Validate(Database database)
    {
        switch (new GrantType().FromString(grant_type))
        {
            case GrantType.AuthorizationCode: 
                Validate_AuthorizationCode(database); 
                break;
            case GrantType.RefreshToken: 
                Validate_RefreshToken(); 
                break;
            case GrantType.Password: 
                Validate_Password(); 
                break;
            case GrantType.ClientCredentials:
                Validate_ClientCredentials();
                break;
            default:
                throw new Exception("grant_type value is not valid for the token input.");
        }
    }

    public void Validate_AuthorizationCode(Database database)
    {
        if (code == null)
            throw new Exception("code is required for Authorization Code flow.");
        if (database.Codes.GetCode(code) == null)
            throw new Exception("code is not valid.");
    }

    public void Validate_RefreshToken()
    {
    }

    public void Validate_Password()
    {
    }

    public void Validate_ClientCredentials()
    {
    }
}
