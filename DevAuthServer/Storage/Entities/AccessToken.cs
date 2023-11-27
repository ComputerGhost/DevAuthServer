using DevAuthServer.Handlers.Authorize;
using Microsoft.AspNetCore.WebUtilities;

namespace DevAuthServer.Storage.Entities;

public class AccessToken
{
    public AccessToken(AuthorizationCode code)
        : this(code.UserId, code.IsOpenId)
    { }

    public AccessToken(AccessToken oldToken)
        : this(oldToken.UserId, oldToken.IsOpenId)
    { }

    public AccessToken(string? userId, bool isOpenId)
    {
        UserId = userId;
        IsOpenId = isOpenId;
        access_token = WebEncoders.Base64UrlEncode(new Guid().ToByteArray());
        refresh_token = WebEncoders.Base64UrlEncode(new Guid().ToByteArray());
    }

    internal string? UserId { get; set; } // Null when Client Credentials grant is used.
    internal bool IsOpenId { get; set; }

    public string access_token { get; set; } = null!;
    public string refresh_token { get; set; } = null!;
}
