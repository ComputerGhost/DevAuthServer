using DevAuthServer.Handlers.Login;

namespace DevAuthServer.Storage.Entities;

public class User
{
    public User (LoginInputModel input)
    {
        DisplayName = input.DisplayName;
        Email = input.Email;
        PictureUrl = input.PictureUrl;
        Roles = input.Roles;
    }

    public string Id { get; set; } = new Guid().ToString();
    public long LoginTime { get; set; } = new DateTime().Ticks / 1000;

    public string DisplayName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PictureUrl { get; set; }

    /// <summary>
    /// Comma-separated list of roles.
    /// </summary>
    public string? Roles { get; set; }
}
