using DevAuthServer.Storage.Entities;

namespace DevAuthServer.Handlers.UserInfo;

public class UserInfoOutputModel
{
    public UserInfoOutputModel(IdToken idToken)
    {
        sub = idToken.sub;
        email = idToken.email;
        email_verified = idToken.email_verified;
        picture = idToken.picture;
        preferred_username = idToken.preferred_username;
        roles = idToken.roles;
    }

    public string sub { get; set; } = null!;
    public string email { get; set; } = null!;
    public bool email_verified { get; set; }
    public string? picture { get; set; }
    public string? preferred_username { get; set; }
    public IEnumerable<string>? roles { get; set; }
}
