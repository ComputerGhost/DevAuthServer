using DevAuthServer.Handlers.Authorize;
using System.Text;

namespace DevAuthServer.Storage.Entities;

/// <summary>
/// Used to authenticate the client with the IdP.
/// This should be exchanged for a token soon.
/// </summary>
public class AuthorizationCode
{
    public AuthorizationCode(AuthorizeInputModel input, string userId)
    {
        code = Convert.ToBase64String(new Guid().ToByteArray());
        ClientId = input.client_id;
        UserId = userId;
        IsOpenId = input.IsOpenId;
        Nonce = input.nonce;
    }

    internal string ClientId { get; set; } = null!;
    internal string UserId { get; set; } = null!;
    internal bool IsOpenId { get; set; }
    internal string? Nonce { get; set; }

    public string code { get; set; } = null!;
}
