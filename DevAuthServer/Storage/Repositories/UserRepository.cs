using DevAuthServer.Handlers.Login;
using DevAuthServer.Storage.Entities;

namespace DevAuthServer.Storage.Repositories;

public class UserRepository
{
    private readonly Dictionary<string, UserEntity> _byId = new();

    public UserEntity CreateUser(LoginIOModel input)
    {
        var entity = new UserEntity
        {
            DisplayName = input.DisplayName,
            Email = input.Email,
            PictureUrl = input.PictureUrl,
            Roles = input.Roles,
        };
        _byId.Add(entity.Id, entity);
        return entity;
    }

    public UserEntity? GetUser(string userId)
    {
        return _byId.TryGetValue(userId, out UserEntity? entity) ? entity : null;
    }
}
