namespace DevAuthServer.Handlers.Discovery;

public class DiscoveryOutputModel
{
    // From <https://openid.net/specs/openid-connect-discovery-1_0.html>:
    public string issuer { get; set; } = null!;
    public string authorization_endpoint { get; set; } = null!;
    public string token_endpoint { get; set; } = null!;
    public string userinfo_endpoint { get; set; } = null!;
    public string jwks_uri { get; set; } = null!;
    public IEnumerable<string> response_types_supported { get; set; } = null!;
    public IEnumerable<string> grant_types_supported { get; set; } = null!;
    public IEnumerable<string> subject_types_supported { get; set; } = null!;
    public IEnumerable<string> id_token_signing_alg_values_supported { get; set; } = null!;
    public IEnumerable<string> token_endpoint_auth_methods_supported { get; set; } = null!;

    // From other specs:
    public string check_session_iframe { get; set; } = null!;
    public string end_session_endpoint { get; set; } = null!;
    public string introspection_endpoint { get; set; } = null!;
}
