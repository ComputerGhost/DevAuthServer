namespace DevAuthServer.Storage;

public class Browser
{
    private HttpRequest _request;
    private HttpResponse _response;

    public Browser(HttpRequest request, HttpResponse response)
    {
        _request = request;
        _response = response;
    }

    public string? UserIdCookie
    {
        get
        {
            return _request.Cookies["user-id"];
        }
        set
        {
            if (value == null)
            {
                _response.Cookies.Delete("user-id");
            }
            else
            {
                _response.Cookies.Append("user-id", value);
            }
        }
    }

    public (string, string)? BasicAuth
    {
        get
        {
            var headerValue = _request.Headers.Authorization.LastOrDefault();
            if (headerValue == null || !headerValue.StartsWith("basic ", StringComparison.CurrentCultureIgnoreCase))
            {
                return null;
            }

            var credentials = headerValue.Substring("basic ".Length);
            var parts = Todo.Base64UrlDecode(credentials).Split(':');
            return parts.Length == 1 ? (parts[0], parts[1]) : null;
        }
    }

    public string? BearerToken
    {
        get
        {
            var authorization = _request.Headers.Authorization.LastOrDefault();
            if (authorization != null && authorization.StartsWith("bearer ", StringComparison.CurrentCultureIgnoreCase))
            {
                return authorization.Substring("bearer ".Length);
            }
            return null;
        }
    }
}
