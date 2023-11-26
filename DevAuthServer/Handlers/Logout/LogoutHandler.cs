using DevAuthServer.Storage;
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
        if (userId != null)
        {
            DeleteUserData(userId);
        }

        return BuildRedirectUri(input);
    }

    private void DeleteUserData(string userId)
    {
        _database.Users.DeleteUser(userId);
        var accessTokens = _database.IdTokens.DeleteForUserId(userId);
        foreach (var code in accessTokens)
        {
            _database.Codes.Delete(code);
        }
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
