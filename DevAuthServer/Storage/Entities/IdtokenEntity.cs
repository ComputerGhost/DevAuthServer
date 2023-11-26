using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

namespace DevAuthServer.Storage.Entities;

public class IdtokenEntity
{
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
        var payload = JsonConvert.SerializeObject(this);
        return $"{header}.{payload}";
    }
}
