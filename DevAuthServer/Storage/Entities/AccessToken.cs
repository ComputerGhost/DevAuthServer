namespace DevAuthServer.Storage.Entities;

public class AccessToken
{
    public AccessToken(string userId)
    {
        UserId = userId;
        access_token = Todo.Base64UrlEncode(new Guid().ToString());
        refresh_token = Todo.Base64UrlEncode(new Guid().ToString());
    }

    internal string UserId { get; set; }

    public string access_token { get; set; } = null!;

    /// <summary>
    /// Can be used to get a new access token.
    /// </summary>
    public string refresh_token { get; set; } = null!;
}
