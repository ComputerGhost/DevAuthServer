namespace DevAuthServer.Handlers.Logout;

public class LogoutInputModel
{
    public string? post_logout_redirect_uri { get; set; }
    public string? state { get; set; }
}
