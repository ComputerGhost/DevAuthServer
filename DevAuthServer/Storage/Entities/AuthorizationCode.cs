using System.Text;

namespace DevAuthServer.Storage.Entities;

/// <summary>
/// Used to authenticate the client with the IdP.
/// This should be exchanged for a token soon.
/// </summary>
public class AuthorizationCode
{
    public AuthorizationCode(string clientId, string userId)
    {
        code = Convert.ToBase64String(new Guid().ToByteArray());
        ClientId = clientId;
        UserId = userId;
    }

    internal string ClientId { get; set; } = null!;
    internal string UserId { get; set; } = null!;

    public string code { get; set; } = null!;
}
