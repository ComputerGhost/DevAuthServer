using DevAuthServer.Storage;
using DevAuthServer.Storage.Entities;

namespace DevAuthServer.Handlers.Login;

public class LoginHandler
{
    private Database _database;
    private LoginInputModel _input;

    public LoginHandler(Database database, LoginInputModel input)
    {
        _database = database;
        _input = input;
    }

    public string Process()
    {
        var newUser = new User(_input);
        _database.Users.Add(newUser);
        return newUser.Id;
    }
}
