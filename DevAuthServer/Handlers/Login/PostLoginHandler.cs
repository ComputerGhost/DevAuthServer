using DevAuthServer.Storage;

namespace DevAuthServer.Handlers.Login;

public class PostLoginHandler
{
    private Database _database;
    private LoginIOModel _input;

    public PostLoginHandler(Database database, LoginIOModel input)
    {
        _database = database;
        _input = input;
    }

    public string Process()
    {
        var newUser = _database.Users.CreateUser(_input);
        return newUser.Id;
    }
}
