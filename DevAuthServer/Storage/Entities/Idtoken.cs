using DevAuthServer.Handlers.Authorize;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DevAuthServer.Storage.Entities;

public class IdToken
{
    public IdToken(AuthorizeInputModel input, User user, AccessToken token)
    {
        iss = Todo.ISSUER;
        sub = user.Id;
        aud = input.client_id;
        exp = Todo.OIDC_TOKEN_EXPIRES_IN_SECONDS;
        iat = new DateTime().Ticks / 1000; // Time of token creation
        auth_time = user.LoginTime;
        nonce = input.nonce;
        email = user.Email;
        picture = user.PictureUrl;
        preferred_username = user.DisplayName;
        roles = user.Roles?.Split(',');
    }

    // OIDC Core claims from <https://openid.net/specs/openid-connect-core-1_0.html>
    public string iss { get; set; } = null!;
    public string sub { get; set; } = null!;
    public string aud { get; set; } = null!;
    public long exp { get; set; }
    public long iat { get; set; }
    public long auth_time { get; set; }
    public string? nonce { get; set; }

    // Common additional claims from <https://www.iana.org/assignments/jwt/jwt.xhtml>.
    public string email { get; set; } = null!;
    public const bool email_verified = true;
    public string? picture { get; set; }
    public string? preferred_username { get; set; }
    public IEnumerable<string>? roles { get; set; }

    public string Encode()
    {
        var header = "{\"typ\": \"JWT\", \"alg\": \"HS256\"}";
        var encodedHeader = Todo.Base64UrlEncode(header);

        var payload = JsonConvert.SerializeObject(this);
        var encodedPayload = Todo.Base64UrlEncode(payload);

        var secret = Encoding.UTF8.GetBytes(Todo.CLIENT_SECRET);
        var source = Encoding.UTF8.GetBytes(encodedPayload);
        var signature = HMACSHA256.HashData(secret, source);
        var encodedSignature = Todo.Base64UrlEncode(signature);

        return $"{encodedHeader}.{encodedPayload}.{encodedSignature}";
    }
}
