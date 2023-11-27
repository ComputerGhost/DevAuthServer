using DevAuthServer.Storage;

namespace DevAuthServer.Handlers.UserInfo;

public class UserInfoHandler
{
    private readonly Database _database;
    private readonly UserInfoInputModel _input;

    public UserInfoHandler(Database database, UserInfoInputModel input)
    {
        _database = database;
        _input = input;
    }

    public UserInfoOutputModel Process()
    {
        var accessToken = _database.AccessTokens.Single(t => t.access_token == _input.access_token);
        var idToken = _database.IdTokens.Last(t => t.sub == accessToken.UserId);
        return new UserInfoOutputModel(idToken);
    }
}
