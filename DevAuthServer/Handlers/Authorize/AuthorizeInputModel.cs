using DevAuthServer.Constants;

namespace DevAuthServer.Handlers.Authorize;

public class AuthorizeInputModel
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

    internal bool IsOpenId => Scopes.Contains("openid");

    internal FlowType Flow => response_type switch
    {
        "code" => FlowType.AuthorizationCode,
        "id_token" => FlowType.Implicit,
        "id_token token" => FlowType.Implicit,
        "token" => FlowType.Implicit,
        "code token" => FlowType.Hybrid,
        "code id_token" => FlowType.Hybrid,
        "code id_token token" => FlowType.Hybrid,
        _ => FlowType.Invalid,
    };

    internal IEnumerable<string> Scopes => (scope ?? "").Split(' ');

    internal void Validate()
    {
        // Validate client_id
        if (client_id == null)
            throw new Exception("client_id is required.");

        // Validate code_challenge
        if (code_challenge != null && !new[] { "plain", "S256" }.Contains(code_challenge_method))
            throw new Exception("code_challenge_method is not valid.");

        //  Validate nonce
        if (Flow == FlowType.Implicit && nonce == null)
            throw new Exception("nonce is required for implicit flow.");

        // Validate redirect_uri
        if (IsOpenId && redirect_uri == null)
            throw new Exception("redirect_uri is required for openid.");

        // Validate response_type
        if (Flow == FlowType.Invalid)
            throw new Exception("response_type is not valid.");
        if (response_type.Contains("id_token") && !IsOpenId)
            throw new Exception("response_type requires an openid scope.");
    }
}
