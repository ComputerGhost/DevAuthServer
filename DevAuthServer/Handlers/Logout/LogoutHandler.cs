using DevAuthServer.Storage;
using DevAuthServer.Storage.Entities;
using System.Web;

namespace DevAuthServer.Handlers.Logout;

public class LogoutHandler
{
    private readonly Database _database;

    public LogoutHandler(Database database)
    {
        _database = database;
    }

    public Uri? Process(LogoutInputModel input, string? userId)
    {
        _database.AuthorizationCodes.RemoveAll(c => c.UserId == userId);
        _database.AccessTokens.RemoveAll(t => t.UserId == userId);
        _database.IdTokens.RemoveAll(t => t.sub == userId);
        _database.Users.RemoveAll(u => u.Id == userId);

        return BuildRedirectUri(input);
    }

    private Uri? BuildRedirectUri(LogoutInputModel input)
    {
        if (input.post_logout_redirect_uri == null)
        {
            return null;
        }

        var uriBuilder = new UriBuilder(input.post_logout_redirect_uri);
        if (input.state != null)
        {
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query.Add("state", input.state);
            uriBuilder.Query = query.ToString();
        }

        return uriBuilder.Uri;
    }
}
