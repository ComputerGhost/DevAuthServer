using DevAuthServer.Handlers.Authorize;
using DevAuthServer.Storage.Entities;

namespace DevAuthServer.Storage.Repositories;

public class IdTokenRepository
{
    private readonly Dictionary<string, IdtokenEntity> _byAccessToken = new();

    public IdtokenEntity CreateIdToken(GetAuthorizeIOModel input, ClientEntity client)
    {
    }

    public IdtokenEntity? GetIdToken(string access_token)
    {
        return _byAccessToken.TryGetValue(access_token, out IdtokenEntity? token) ? token : null;
    }
}
