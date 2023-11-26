using DevAuthServer.Handlers.Authorize;
using DevAuthServer.Storage.Entities;

namespace DevAuthServer.Storage.Repositories;

public class TokenRepository
{
    private readonly Dictionary<string, TokenEntity> _byAccessToken = new();

    public TokenEntity CreateToken()
    {
        var entity = new TokenEntity(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        _byAccessToken.Add(entity.access_token, entity);
        return entity;
    }

    public TokenEntity? GetToken(string access_token)
    {
        return _byAccessToken.TryGetValue(access_token, out TokenEntity? token) ? token : null;
    }
}
