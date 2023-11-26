using DevAuthServer.Storage.Entities;

namespace DevAuthServer.Storage;

public class Database
{
    public readonly List<AuthorizationCode> AuthorizationCodes = new();
    public readonly List<IdToken> IdTokens = new();
    public readonly List<AccessToken> AccessTokens = new();
    public readonly List<User> Users = new();
}
