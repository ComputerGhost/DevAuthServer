using DevAuthServer.Handlers.Authorize;
using System.Security.Cryptography;
using System.Text;

namespace DevAuthServer;

// These are things I still need to implement.
// Maybe I've implemented them here.. then they still need tested and moved.
public static class Todo
{
    public static int OIDC_TOKEN_EXPIRES_IN_SECONDS { get; set; } = 60 * 60;

    public static string ISSUER = "localhost";
    public static string USER_ID = "userid";

    public static string USERID_COOKIE_NAME = "user-id";

    public static void ProcessIntrospection() => throw new NotImplementedException();

    // See <https://openid.net/specs/openid-connect-session-1_0.html>.
    #region OIDC_SESSION_MANAGEMENT

    public static bool ENABLE_OIDC_SESSION_MANAGEMENT = false;

    public static string GenerateSessionState(string redirect_uri, string client_id)
    {
        // I'm not sure how to get the browser session key yet, so...
        var sessionKey = "abcdefghiujklmnopqrstvwxyz";

        var origin = new Uri(redirect_uri).GetLeftPart(UriPartial.Authority);
        var unencoded_state = $"{client_id} {origin} {sessionKey} abc";
        var encoded_state = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(unencoded_state)));
        return $"{encoded_state}.abc";
    }

    #endregion
}
