namespace DevAuthServer.Constants;

public class OIDCConfig
{
    public static OIDCConfig Instance { get; set; } = null!; // set at startup

    public string ClientSecret { get; set; } = null!;
    public string Issuer { get; set; } = null!;

    /// <summary>
    /// Token expiration in seconds.
    /// </summary>
    public int TokenExpiration { get; set; }
}
