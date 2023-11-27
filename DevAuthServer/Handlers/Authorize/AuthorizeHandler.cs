using DevAuthServer.Constants;
using DevAuthServer.Handlers.Login;
using DevAuthServer.Storage;
using DevAuthServer.Storage.Entities;
using System;
using System.Web;

namespace DevAuthServer.Handlers.Authorize;

public class AuthorizeHandler
{
    private Database _database;
    private AuthorizeInputModel _input;
    private string _userId;

    public AuthorizeHandler(Database database, AuthorizeInputModel input, string userId)
    {
        _database = database;
        _input = input;
        _userId = userId;
    }

    public Uri Process()
    {
        return _input.Flow switch
        {
            FlowType.AuthorizationCode => Process_AuthorizationCode(),
            FlowType.Hybrid => Process_Hybrid(),
            FlowType.Implicit => Process_Implicit(),
            _ => new Uri("/"),
        };
    }

    private Uri Process_AuthorizationCode()
    {
        // Create a code to reference the user and client.
        var newCode = new AuthorizationCode(_input, _userId);
        _database.AuthorizationCodes.Add(newCode);

        // We'll be modifying the redirect_uri to include our response.
        var uriBuilder = new UriBuilder(_input.redirect_uri!);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query["code"] = newCode.code;
        if (_input.state != null)
            query["state"] = _input.state;
        if (Todo.ENABLE_OIDC_SESSION_MANAGEMENT)
            query["session_state"] = Todo.GenerateSessionState(_input.redirect_uri!, _input.client_id);

        uriBuilder.Query = query.ToString();
        return uriBuilder.Uri;
    }

    private Uri Process_Hybrid()
    {
        // Do both other methods.
        var codeUri = Process_AuthorizationCode();
        var implicitUri = Process_Implicit();

        // Now combine the responses.
        var uriBuilder = new UriBuilder(codeUri);
        uriBuilder.Fragment = implicitUri.Fragment;
        return uriBuilder.Uri;
    }

    private Uri Process_Implicit()
    {
        // Create an access token.
        var accessToken = new AccessToken(_userId);

        // We'll be modifying the redirect_uri to include our response.
        var uriBuilder = new UriBuilder(_input.redirect_uri!);
        var fragments = HttpUtility.ParseQueryString("");

        fragments["token_type"] = "bearer";
        fragments["expires_in"] = Todo.OIDC_TOKEN_EXPIRES_IN_SECONDS.ToString();
        if (_input.state != null)
            fragments["state"] = _input.state;
        if (Todo.ENABLE_OIDC_SESSION_MANAGEMENT)
            fragments["session_state"] = Todo.GenerateSessionState(_input.redirect_uri!, _input.client_id);

        // If user requested the token returned, then return it
        if (_input.response_type.Split(' ').Contains("token"))
            fragments["access_token"] = accessToken.access_token;

        // If we're doing openid, then create an id token too.
        if (_input.IsOpenId)
        {
            var user = _database.Users.First(u => u.Id == _userId);
            var idToken = new IdToken(_input, user);
            _database.IdTokens.Add(idToken);

            // If the user requested the id token returned, then return it.
            if (_input.response_type.Split(' ').Contains("id_token"))
                fragments["id_token"] = idToken.Encode();
        }

        uriBuilder.Fragment = fragments.ToString();
        return uriBuilder.Uri;
    }
}
