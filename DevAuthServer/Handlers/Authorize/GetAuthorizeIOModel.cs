using DevAuthServer.Constants;
using DevAuthServer.Storage;

namespace DevAuthServer.Handlers.Authorize;

public class GetAuthorizeIOModel
{
    public string client_id { get; set; } = null!;
    public string? code_challenge { get; set; }
    public string? code_challenge_method { get; set; }
    public string? nonce { get; set; }
    public string? prompt { get; set; }
    public string? redirect_uri { get; set; }
    public string response_type { get; set; } = null!;
    public string? scope { get; set; }
    public string? state { get; set; }

    internal IEnumerable<string> Scopes => (scope ?? "").Split(',');

    internal void Validate(Database database)
    {
        var isOpenId = Scopes.Contains("openid");
        var grantType = new GrantType().FromResponseType(response_type);

        // Validate client_id
        var client = database.Clients.GetClient(client_id)
            ?? throw new Exception("client_id did not match to a client.");

        // Validate code_challenge
        if (code_challenge != null && !new[] { "plain", "S256" }.Contains(code_challenge_method))
            throw new Exception("code_challenge_method is not valid.");

        //  Validate nonce
        if (grantType == GrantType.Implicit && nonce == null)
            throw new Exception("nonce is required for implicit flow.");

        // Validate redirect_uri
        if (isOpenId && redirect_uri == null)
            throw new Exception("redirect_uri is required for openid.");
        if (isOpenId && client.RedirectUris.Contains(redirect_uri))
            throw new Exception("redirect_uri is not valid for the client.");

        // Validate response_type
        if (grantType == GrantType.None)
            throw new Exception("response_type is not valid.");
        if (response_type.Contains("id_token") && !isOpenId)
            throw new Exception("response_type requires an openid scope.");
    }
}
