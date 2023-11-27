using DevAuthServer.Storage.Entities;

namespace DevAuthServer.Handlers.Introspect;

public class IntrospectOutputModel
{
    public IntrospectOutputModel(IntrospectInputModel input, IdToken idToken)
    {
        iss = idToken.iss;
        sub = idToken.sub;
        aud = idToken.aud;
        exp = idToken.exp;
        iat = idToken.iat;
        client_id = input.client_id;
    }

    public string iss { get; set; } = null!;
    public string sub { get; set; } = null!;
    public string aud { get; set; } = null!;
    public long exp { get; set; }
    public long iat { get; set; }
    public bool active { get; set; } = true;
    public string client_id { get; set; } = null!;
    public string scope { get; set; } = "openid everything";
}
