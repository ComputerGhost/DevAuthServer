using DevAuthServer.Constants;
using DevAuthServer.Storage;
using DevAuthServer.Storage.Entities;

namespace DevAuthServer.Handlers.Token;

public class TokenHandler
{
    private readonly Database _database;
    private readonly TokenInputModel _input;

    public TokenHandler(Database database, TokenInputModel input)
    {
        _database = database;
        _input = input;
    }

    public TokenOutputModel Process()
    {
        var grantType = new GrantType().FromString(_input.grant_type);
        return grantType switch
        {
            GrantType.AuthorizationCode => Process_AuthorizationCode(),
            GrantType.RefreshToken => Process_RefreshToken(),
            GrantType.Password => Process_Password(),
            GrantType.ClientCredentials => Process_ClientCredentials(),
            _ => throw new Exception("grant_type value is not valid for token endpoint.")
        };
    }

    private TokenOutputModel Process_AuthorizationCode()
    {
        var response = new TokenOutputModel();

        var code = _database.AuthorizationCodes.First(c => c.code == _input.code);

        // We'll always want to make an access token.
        var accessToken = new AccessToken(code);
        _database.AccessTokens.Add(accessToken);
        response.access_token = accessToken.access_token;
        response.refresh_token = accessToken.refresh_token;

        // For OpenId, we also make an id token.
        if (code.IsOpenId)
        {
            var user = _database.Users.Single(u => u.Id == code.UserId);
            var idToken = new IdToken(_input, user, code);
            _database.IdTokens.Add(idToken);
            response.id_token = idToken.Encode();
        }

        // We no longer need the authorization code.
        _database.AuthorizationCodes.RemoveAll(c => c.code == _input.code);

        return response;
    }

    private TokenOutputModel Process_RefreshToken()
    {
        var response = new TokenOutputModel();

        var oldToken = _database.AccessTokens.First(t => t.refresh_token == _input.refresh_token);

        // We'll always want to make an access token.
        var accessToken = new AccessToken(oldToken);
        _database.AccessTokens.Add(accessToken);
        response.access_token = accessToken.access_token;
        response.refresh_token = accessToken.refresh_token;

        // For OpenId, we also make an id token.
        if (oldToken.IsOpenId)
        {
            var user = _database.Users.Single(u => u.Id == oldToken.UserId);
            var idToken = new IdToken(_input, user);
            _database.IdTokens.Add(idToken);
            response.id_token = idToken.Encode();
        }

        // Delete the old access token now.
        _database.AccessTokens.Remove(oldToken);

        return response;
    }

    private TokenOutputModel Process_Password()
    {
        var response = new TokenOutputModel();

        var user = _database.Users.Single(u => u.Email == _input.username);

        // We want to make an access token.
        var accessToken = new AccessToken(user.Id, true);
        _database.AccessTokens.Add(accessToken);
        response.access_token = accessToken.access_token;
        response.refresh_token = accessToken.refresh_token;

        // For OpenId, we also make an id token.
        var idToken = new IdToken(_input, user);
        _database.IdTokens.Add(idToken);
        response.id_token = idToken.Encode();
        response.scope = _input.scope;

        return response;
    }

    private TokenOutputModel Process_ClientCredentials()
    {
        var response = new TokenOutputModel();

        // Just create an access token here.
        var accessToken = new AccessToken(null, false);
        _database.AccessTokens.Add(accessToken);
        response.access_token = accessToken.access_token;
        response.refresh_token = accessToken.refresh_token;
        response.scope = _input.scope;

        return response;
    }
}
