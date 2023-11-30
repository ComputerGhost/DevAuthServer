using DevAuthServer.Storage;
using Microsoft.AspNetCore.Mvc;

namespace DevAuthServer.Handlers.Introspect;

public class IntrospectInputModel
{
    public string token { get; set; } = null!;
    public string client_id { get; set; } = null!;
    public string client_secret { get; set; } = null!;

    public void Validate(Database database)
    {
        if (!database.AccessTokens.Any(t => t.access_token == token))
            throw new Exception("token does not exist.");
    }
}
