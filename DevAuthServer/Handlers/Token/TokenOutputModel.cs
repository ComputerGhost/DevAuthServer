namespace DevAuthServer.Handlers.Token;

public class TokenOutputModel
{
    // These are always returned.
    public string access_token { get; set; } = null!;
    public string refresh_token { get; set; } = null!;
    public string token_type { get; set; } = "Bearer";
    public int expires_in { get; set; } = Todo.OIDC_TOKEN_EXPIRES_IN_SECONDS;

    // This is only set if using OpenID.
    public string? id_token { get; set; }

    // This is only set when using password or client_credentials grant.
    public string? scope { get; set; }
}
