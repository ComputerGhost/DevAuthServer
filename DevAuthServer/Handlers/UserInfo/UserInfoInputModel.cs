using DevAuthServer.Storage;

namespace DevAuthServer.Handlers.UserInfo;

public class UserInfoInputModel
{
    public string? access_token { get; set; }

    internal string? fromHeader { get; set; }
    internal string? fromQuery { get; set; }
    internal string? fromBody { get; set; }

    public void Normalize()
    {
        fromBody = access_token;
        access_token = fromHeader ?? fromBody ?? fromQuery;
    }

    public void Validate(Database database)
    {
        int count = 0;
        count += fromHeader != null ? 1 : 0;
        count += fromQuery != null ? 1 : 0;
        count += fromBody != null ? 1 : 0;

        if (count == 0)
            throw new Exception("access_token is required.");
        if (count > 1)
            throw new Exception("access_token may only be delivered in one way.");

        if (!database.AccessTokens.Any(t => t.access_token == access_token))
            throw new Exception("access_token does not exist.");
    }
}
