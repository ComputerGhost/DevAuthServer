using DevAuthServer.Handlers.Authorize;

namespace DevAuthServer.Handlers.Login;

public class LoginInputModel : AuthorizeInputModel
{
    public string DisplayName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PictureUrl { get; set; }

    /// <summary>
    /// Comma-separated list of roles.
    /// </summary>
    public string? Roles { get; set; }
}
