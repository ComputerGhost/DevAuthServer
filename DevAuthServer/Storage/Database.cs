using DevAuthServer.Storage.Entities;
using DevAuthServer.Storage.Repositories;

namespace DevAuthServer.Storage;

public class Database
{
    public readonly ClientRepository Clients = new();
    public readonly CodeRepository Codes = new();
    public readonly IdTokenRepository IdTokens = new();
    public readonly TokenRepository Tokens = new();
    public readonly UserRepository Users = new();
}
