namespace DevAuthServer.Storage.Entities;

public class TokenEntity
{
    public TokenEntity(string accessToken, string refreshToken)
    {
        access_token = accessToken;
        refresh_token = refreshToken;
    }

    public string access_token { get; set; } = null!;
    public string refresh_token { get; set; } = null!;
}
