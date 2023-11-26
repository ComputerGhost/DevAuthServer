using DevAuthServer.Constants;
using DevAuthServer.Storage;
using System;
using System.Web;

namespace DevAuthServer.Handlers.Authorize;

public class PostAuthorizeHandler
{
    private Database _database;
    private GetAuthorizeIOModel _input;

    public PostAuthorizeHandler(GetAuthorizeIOModel input, Database database)
    {
        _input = input;
        _database = database;
    }

    public Uri Process()
    {
        var grantType = new GrantType().FromResponseType(_input.response_type);
        return grantType switch
        {
            GrantType.AuthorizationCode => Process_AuthorizationCode(),
            GrantType.Hybrid => Process_Hybrid(),
            GrantType.Implicit => Process_Implicit(),
            _ => new Uri("/"),
        };
    }

    private Uri Process_AuthorizationCode()
    {
        var uriBuilder = new UriBuilder(_input.redirect_uri!);

        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["code"] = _database.Codes.CreateCode().Code;
        if (_input.state != null)
            query["state"] = _input.state;
        if (Todo.ENABLE_OIDC_SESSION_MANAGEMENT)
            query["session_state"] = Todo.GenerateSessionState(_input.redirect_uri!, _input.client_id);
        uriBuilder.Query = query.ToString();

        return uriBuilder.Uri;
    }

    private Uri Process_Hybrid()
    {
        var codeUri = Process_AuthorizationCode();
        var implicitUri = Process_Implicit();

        var uriBuilder = new UriBuilder(codeUri);
        uriBuilder.Fragment = implicitUri.Fragment;
        return uriBuilder.Uri;
    }

    private Uri Process_Implicit()
    {
        var uriBuilder = new UriBuilder(_input.redirect_uri!);
        var fragments = HttpUtility.ParseQueryString("");

        var token = _database.Tokens.CreateToken();

        if (_input.response_type.Split(' ').Contains("token"))
            fragments["access_token"] = token.access_token;

        if (_input.Scopes.Contains("openid"))
        {
            var client = _database.Clients.GetClient(_input.client_id)!;
            var id_token = _database.IdTokens.CreateIdToken(_input, client);
            if (_input.response_type.Split(' ').Contains("id_token"))
                fragments["id_token"] = id_token.Encode();
        }

        fragments["token_type"] = "bearer";
        fragments["expires_in"] = Todo.OIDC_TOKEN_EXPIRES_IN_SECONDS.ToString();
        if (_input.state != null)
            fragments["state"] = _input.state;
        if (Todo.ENABLE_OIDC_SESSION_MANAGEMENT)
            fragments["session_state"] = Todo.GenerateSessionState(_input.redirect_uri!, _input.client_id);

        uriBuilder.Fragment = fragments.ToString();
        return uriBuilder.Uri;
    }
}
