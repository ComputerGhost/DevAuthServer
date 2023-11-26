namespace DevAuthServer.Handlers.Token;

public class TokenOutputModel
{
    public string access_token { get; set; }
    public const string token_type = "Bearer";
    public string refresh_token { get; set; }
    public int expires_in { get; set; }
    public string id_token { get; set; }
}
