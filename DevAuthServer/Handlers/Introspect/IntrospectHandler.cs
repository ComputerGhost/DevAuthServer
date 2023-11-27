using DevAuthServer.Storage;

namespace DevAuthServer.Handlers.Introspect;

public class IntrospectHandler
{
    private readonly Database _database;
    private readonly IntrospectInputModel _input;

    public IntrospectHandler(Database database, IntrospectInputModel input)
    {
        _database = database;
        _input = input;
    }

    public IntrospectOutputModel Process()
    {
        var accessToken = _database.AccessTokens.Single(t => t.access_token == _input.token);
        var idToken = _database.IdTokens.Last(t => t.sub == accessToken.UserId);
        return new IntrospectOutputModel(_input, idToken);
    }
}
