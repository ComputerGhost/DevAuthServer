using System.Security.Cryptography.X509Certificates;

namespace DevAuthServer.Storage.Entities;

public class IdtokenEntity
{
    public string iss { get; set; }
    public string sub { get; set; }
    public string aud { get; set; }
    public long exp { get; set; }
    public string auth_time { get; set; }
    public string? nonce { get; set; }
    public string? at_hash { get; set; }

    public string Encode()
    {
    }
}
