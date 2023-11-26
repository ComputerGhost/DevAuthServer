using DevAuthServer.Constants;
using DevAuthServer.Handlers.Authorize;
using DevAuthServer.Handlers.Login;
using DevAuthServer.Storage.Entities;

namespace DevAuthServer.Storage.Repositories;

public class IdTokenRepository
{
    private readonly Dictionary<string, IdtokenEntity> _byAccessToken = new();

    public IdtokenEntity CreateIdToken(
        GetAuthorizeIOModel input, 
        ClientEntity client,
        UserEntity user,
        TokenEntity accessToken)
    {
        var token = new IdtokenEntity
        {
            iss = Todo.ISSUER,
            sub = user.Id,
            aud = client.ClientId,
            exp = Todo.OIDC_TOKEN_EXPIRES_IN_SECONDS,
            iat = new DateTime().Ticks / 1000, // Time of token creation
            auth_time = user.LoginTime,
            nonce = input.nonce,
            email = user.Email,
            picture = user.PictureUrl,
            preferred_username = user.DisplayName,
            roles = user.Roles?.Split(','),
        };
        _byAccessToken.Add(accessToken.access_token, token);
        return token;
    }

    /// <summary>
    /// Removes all id tokens associated with a user.
    /// </summary>
    /// <returns>All access tokens associated with deleted id tokens.</returns>
    public IEnumerable<string> DeleteForUserId(string userId)
    {
        var accessTokens = _byAccessToken.Where(kvp => kvp.Value.sub == userId).Select(kvp => kvp.Key);
        foreach (var accessToken in accessTokens)
        {
            _byAccessToken.Remove(accessToken);
        }
        return accessTokens;
    }

    public IdtokenEntity? GetIdToken(string access_token)
    {
        return _byAccessToken.TryGetValue(access_token, out IdtokenEntity? token) ? token : null;
    }
}
