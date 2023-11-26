using DevAuthServer.Constants;
using DevAuthServer.Storage;

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
        }; ;
    }

    public TokenOutputModel Process_AuthorizationCode()
    {
        Todo.DoSomething();
        return new TokenOutputModel();
    }

    public TokenOutputModel Process_RefreshToken()
    {
        Todo.DoSomething();
        return new TokenOutputModel();
    }

    public TokenOutputModel Process_Password()
    {
        Todo.DoSomething();
        return new TokenOutputModel();
    }

    public TokenOutputModel Process_ClientCredentials()
    {
        Todo.DoSomething();
        return new TokenOutputModel();
    }
}
