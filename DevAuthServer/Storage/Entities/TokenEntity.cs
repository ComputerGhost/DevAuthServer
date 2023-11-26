namespace DevAuthServer.Storage.Entities;

public class TokenEntity
{
    public TokenEntity(string accessToken)
    {
        access_token = accessToken;
    }

    public string access_token { get; set; } = null!;
}
